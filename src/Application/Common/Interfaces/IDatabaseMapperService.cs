using DataVision.Application.Common.Models;

namespace DataVision.Application.Common.Interfaces;
public interface IDatabaseMapperService
{
    Task<DatabaseMappingResult> MapDatabase(int databaseId, List<FetchedTable> fetchedTables, CancellationToken cancellationToken = default);
}
