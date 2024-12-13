using DataVision.Application.Common.Interfaces;
using DataVision.Application.Common.Jobs;
using DataVision.Domain.Entities;

namespace DataVision.Application.Databases.Commands.ClearDatabase;

public record ClearDatabaseCommand : IRequest<int>
{
    public int Id { get; init; }
}

public class ClearDatabaseCommandHandler : IRequestHandler<ClearDatabaseCommand, int>
{
    private readonly IApplicationDbContext _context;

    public ClearDatabaseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(ClearDatabaseCommand request, CancellationToken cancellationToken)
    {
        var job = new BackgroundJob();
        _context.BackgroundJobs.Add(job);
        await _context.SaveChangesAsync(cancellationToken);

        var externalJobId = Hangfire.BackgroundJob.Enqueue<ClearDatabaseJob>(x => x.Run(job.Id, request.Id, false, cancellationToken));

        job.ExternalJobId = externalJobId;
        _context.BackgroundJobs.Update(job);
        await _context.SaveChangesAsync(cancellationToken);

        return job.Id;
    }
}
