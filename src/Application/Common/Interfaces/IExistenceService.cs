namespace DataVision.Application.Common.Interfaces;
public interface IExistenceService
{
    Task<bool> DatabaseExistsAsync(int databaseId, CancellationToken cancellationToken = default);
    Task<bool> DatabaseTableExistsAsync(int databaseTableId, CancellationToken cancellationToken = default);
}
