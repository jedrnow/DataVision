namespace DataVision.Application.Common.Interfaces;
public interface IExistenceService
{
    Task<bool> DatabaseExistsAsync(int databaseId, CancellationToken cancellationToken = default);
}
