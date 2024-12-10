using DataVision.Application.Common.Factories;
using DataVision.Application.Common.Interfaces;
using DataVision.Application.Common.Models;
using DataVision.Domain.Entities;
using Newtonsoft.Json;

namespace DataVision.Application.Common.Jobs;
public class PopulateDatabaseJob
{
    private readonly IApplicationDbContext _context;
    private readonly IDatabaseMapperService _dbMapperService;
    public PopulateDatabaseJob(IApplicationDbContext context, IDatabaseMapperService dbMapperService)
    {
        _context = context;
        _dbMapperService = dbMapperService;
    }

    public async Task Run(int jobId, int databaseId, CancellationToken cancellationToken = default)
    {
        var job = await _context.BackgroundJobs.SingleOrDefaultAsync(x => x.Id == jobId, cancellationToken);
        Guard.Against.NotFound(jobId, job);

        var database = await _context.Databases.SingleOrDefaultAsync(x => x.Id == databaseId, cancellationToken);

        Guard.Against.NotFound(databaseId, database);
        Guard.Against.NullOrEmpty(database.ConnectionString, parameterName: nameof(Database.ConnectionString));

        var dbAdapter = DatabaseAdapterFactory.CreateAdapter(database.Provider, database.ConnectionString);

        var connectionIsAvailable = await dbAdapter.CanConnectAsync();
        if (!connectionIsAvailable)
        {
            throw new InvalidOperationException("Cannot connect to given database.");
        }

        var databaseFetchingResult = await dbAdapter.FetchDatabaseAsync();
        if (databaseFetchingResult == null || !databaseFetchingResult.Success)
        {
            var failResult = new DatabaseMappingResult()
            {
                Success = false,
                DatabaseName = database.Name ?? "",
                Errors = databaseFetchingResult?.Errors ?? []
            };

            job.Result = JsonConvert.SerializeObject(failResult);
        }
        else
        {
            var result = await _dbMapperService.MapDatabase(databaseId, databaseFetchingResult.Tables);
            if (result.Success)
            {
                database.IsPopulated = true;
                job.IsSucceeded = true;
            }

            job.Result = JsonConvert.SerializeObject(result);
        }

        job.IsCompleted = true;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
