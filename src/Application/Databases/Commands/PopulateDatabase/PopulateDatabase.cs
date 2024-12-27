using DataVision.Application.Common.Interfaces;
using DataVision.Application.Common.Jobs;
using DataVision.Domain.Entities;

namespace DataVision.Application.Databases.Commands.PopulateDatabase;
public record PopulateDatabaseCommand : IRequest<int>
{
    public int DatabaseId { get; set; }
}

public class PopulateDatabaseCommandHandler : IRequestHandler<PopulateDatabaseCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _user;

    public PopulateDatabaseCommandHandler(IApplicationDbContext context, IUser user)
    {
        _context = context;
        _user = user;
    }

    public async Task<int> Handle(PopulateDatabaseCommand request, CancellationToken cancellationToken)
    {
        var database = await _context.Databases.SingleOrDefaultAsync(x => x.Id == request.DatabaseId, cancellationToken);

        Guard.Against.NotFound(request.DatabaseId, database);
        Guard.Against.NullOrEmpty(database.ConnectionString, parameterName: nameof(Database.ConnectionString));

        var job = new BackgroundJob();
        _context.BackgroundJobs.Add(job);
        await _context.SaveChangesAsync(cancellationToken);

        var externalJobId = Hangfire.BackgroundJob.Enqueue<PopulateDatabaseJob>(x => x.Run(job.Id, _user.Id, request.DatabaseId, cancellationToken));

        job.ExternalJobId = externalJobId;
        _context.BackgroundJobs.Update(job);
        await _context.SaveChangesAsync(cancellationToken);

        return job.Id;
    }
}

