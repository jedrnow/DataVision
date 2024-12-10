using DataVision.Application.Common.Interfaces;

namespace DataVision.Application.Common.Jobs;
public class ClearDatabaseJob
{
    private readonly IApplicationDbContext _context;
    public ClearDatabaseJob(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Run(int jobId, int databaseId, bool deleteAfterwards = false, CancellationToken cancellationToken = default)
    {
        var job = await _context.BackgroundJobs.SingleOrDefaultAsync(x => x.Id == jobId, cancellationToken);
        Guard.Against.NotFound(jobId, job);

        var database = await _context.Databases.SingleOrDefaultAsync(x => x.Id == databaseId, cancellationToken);

        Guard.Against.NotFound(databaseId, database);

        var cells = await _context.DatabaseTableCells
            .Where(x => x.DatabaseId == database.Id)
            .ExecuteDeleteAsync(cancellationToken);

        var rows = await _context.DatabaseTableRows
            .Where(x => x.DatabaseId == database.Id)
            .ExecuteDeleteAsync(cancellationToken);

        var columns = await _context.DatabaseTableColumns
            .Where(x => x.DatabaseId == database.Id)
            .ExecuteDeleteAsync(cancellationToken);

        var tables = await _context.DatabaseTables
            .Where(x => x.DatabaseId == database.Id)
            .ExecuteDeleteAsync(cancellationToken);

        if (deleteAfterwards)
        {
            _context.Databases.Remove(database);
        }

        job.IsCompleted = true;
        job.IsSucceeded = true;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
