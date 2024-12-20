using Azure.Storage.Blobs;
using DataVision.Application.Common.Interfaces;
using DataVision.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace DataVision.Application.Reports.Commands.UpdateReport;

public record UpdateReportCommand : IRequest<bool>
{
    public int Id { get; init; }
    public string? Title { get; init; }
    public IFormFile? File { get; init; }
    public bool OverrideFile { get; init; }
}

public class UpdateReportCommandHandler : IRequestHandler<UpdateReportCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly BlobServiceClient _blobServiceClient;

    public UpdateReportCommandHandler(IApplicationDbContext context, BlobServiceClient blobServiceClient)
    {
        _context = context;
        _blobServiceClient = blobServiceClient;
    }

    public async Task<bool> Handle(UpdateReportCommand request, CancellationToken cancellationToken)
    {
        var report = await _context.Reports.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, report);
        Guard.Against.Null(report.FileName);

        report.Title = request.Title;

        if (request.OverrideFile)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(BlobContainerNames.reports.ToString());

            await containerClient.DeleteBlobAsync(report.FileName);

            if (request.File != null)
            {
                var newFileName = report.DatabaseId + "_" + request.File.FileName;

                var blobClient = containerClient.GetBlobClient(newFileName);
                await using var stream = request.File.OpenReadStream();
                await blobClient.UploadAsync(stream, overwrite: true);

                report.FileName = newFileName;
                report.Format = GetFormat(newFileName);
            }
            else
            {
                report.FileName = string.Empty;
            }
        }

        return (await _context.SaveChangesAsync(cancellationToken) > 0);
    }

    private static ReportFormat GetFormat(string fileName)
    {
        if (fileName.EndsWith(".pdf"))
        {
            return ReportFormat.Pdf;
        }

        if (fileName.EndsWith(".xlsx"))
        {
            return ReportFormat.Xlsx;
        }

        throw new NotImplementedException();
    }
}
