using DataVision.Application.Common.Interfaces;
using DataVision.Application.Common.Jobs;
using DataVision.Domain.Entities;

namespace DataVision.Application.Databases.Commands.DeleteDatabase;

public record DeleteDatabaseCommand : IRequest<int>
{
    public int Id { get; init; }
}

public class DeleteDatabaseCommandHandler : IRequestHandler<DeleteDatabaseCommand, int>
{
    private readonly IApplicationDbContext _context;

    public DeleteDatabaseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(DeleteDatabaseCommand request, CancellationToken cancellationToken)
    {
        var job = new BackgroundJob();
        _context.BackgroundJobs.Add(job);
        await _context.SaveChangesAsync(cancellationToken);

        var externalJobId = Hangfire.BackgroundJob.Enqueue<ClearDatabaseJob>(x => x.Run(job.Id, request.Id, true, cancellationToken));

        job.ExternalJobId = externalJobId;
        _context.BackgroundJobs.Update(job);
        await _context.SaveChangesAsync(cancellationToken);

        return job.Id;
    }
}
