using DataVision.Application.Common.Interfaces;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Configuration;

namespace DataVision.Application.Reports.Commands.DownloadReport;

public record DownloadReportCommand : IRequest<(string FileName, byte[] Content)>
{
    public int ReportId { get; init; }
}

public class DownloadReportCommandHandler : IRequestHandler<DownloadReportCommand, (string FileName, byte[] Content)>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _user;
    private readonly IConfiguration _configuration;

    public DownloadReportCommandHandler(IApplicationDbContext context, IUser user, IConfiguration configuration)
    {
        _context = context;
        _user = user;
        _configuration = configuration;
    }

    public async Task<(string FileName, byte[] Content)> Handle(DownloadReportCommand request, CancellationToken cancellationToken)
    {
        var userId = _user.Id;
        var report = await _context.Reports.AsNoTracking().SingleOrDefaultAsync(x => x.Id == request.ReportId && x.CreatedBy == userId, cancellationToken);

        Guard.Against.NotFound(request.ReportId, report);

        var storageClient = StorageClient.Create();
        var bucketName = _configuration.GetSection("GoogleCloudStorage").GetValue<string>("ContainerName");

        var fileName = report.FileName;
        var memoryStream = new MemoryStream();

        await storageClient.DownloadObjectAsync(bucketName, fileName, memoryStream, cancellationToken: cancellationToken);
        return (fileName!, memoryStream.ToArray());
    }
}
