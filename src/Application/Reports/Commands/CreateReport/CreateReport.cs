using Azure.Storage.Blobs;
using DataVision.Application.Common.Documents;
using DataVision.Application.Common.Interfaces;
using DataVision.Application.Common.Jobs;
using DataVision.Domain.Entities;
using DataVision.Domain.Enums;
using Microsoft.Extensions.Configuration;

namespace DataVision.Application.Reports.Commands.CreateReport;

public record CreateReportCommand : IRequest<int>
{
    public int DatabaseId { get; init; }
    public string? Title { get; init; }
    public List<int> TableIds { get; init; } = [];
    public ReportFormat? Format { get; init; }
}

public class CreateReportCommandHandler : IRequestHandler<CreateReportCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _user;

    public CreateReportCommandHandler(IApplicationDbContext context, IUser user)
    {
        _context = context;
        _user = user;
    }

    public async Task<int> Handle(CreateReportCommand request, CancellationToken cancellationToken)
    {
        var userId = _user.Id;
        var database = await _context.Databases.SingleOrDefaultAsync(x => x.Id == request.DatabaseId, cancellationToken);

        Guard.Against.NotFound(request.DatabaseId, database);
        Guard.Against.NullOrEmpty(request.Title, parameterName: nameof(request.Title));
        Guard.Against.Null(request.Format, parameterName: nameof(request.Format));

        var job = new BackgroundJob();
        _context.BackgroundJobs.Add(job);
        await _context.SaveChangesAsync(cancellationToken);

        var externalJobId = Hangfire.BackgroundJob.Enqueue<CreateReportJob>(x => x.Run(job.Id, userId, request.Title, request.DatabaseId, request.Format, request.TableIds, cancellationToken));

        job.ExternalJobId = externalJobId;
        _context.BackgroundJobs.Update(job);
        await _context.SaveChangesAsync(cancellationToken);

        return job.Id;
    }
}
