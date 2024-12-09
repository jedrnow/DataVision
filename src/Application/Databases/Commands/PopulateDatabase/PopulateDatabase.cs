using DataVision.Application.Common.Factories;
using DataVision.Application.Common.Interfaces;
using DataVision.Application.Common.Models;
using DataVision.Domain.Entities;

namespace DataVision.Application.Databases.Commands.PopulateDatabase;
public record PopulateDatabaseCommand : IRequest<DatabaseMappingResult>
{
    public int DatabaseId { get; set; }
}

public class PopulateDatabaseCommandHandler : IRequestHandler<PopulateDatabaseCommand, DatabaseMappingResult>
{
    private readonly IApplicationDbContext _context;
    private readonly IDatabaseMapperService _dbMapperService;

    public PopulateDatabaseCommandHandler(IApplicationDbContext context, IDatabaseMapperService dbMapperService)
    {
        _context = context;
        _dbMapperService = dbMapperService;
    }

    public async Task<DatabaseMappingResult> Handle(PopulateDatabaseCommand request, CancellationToken cancellationToken)
    {
        var database = await _context.Databases.SingleOrDefaultAsync(x => x.Id == request.DatabaseId, cancellationToken);

        Guard.Against.NotFound(request.DatabaseId, database);
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
            return new DatabaseMappingResult()
            {
                Success = false,
                DatabaseName = database.Name ?? "",
                Errors = databaseFetchingResult?.Errors ?? []
            };
        }

        var result = await _dbMapperService.MapDatabase(request.DatabaseId, databaseFetchingResult.Tables);
        if (result.Success)
        {
            database.IsPopulated = true;
            await _context.SaveChangesAsync(cancellationToken);
        }

        return result;
    }
}

