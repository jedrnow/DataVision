namespace DataVision.Application.Common.Interfaces;
public interface IExistenceService
{
    Task<bool> DatabaseExistsAsync(int databaseId, CancellationToken cancellationToken = default);
    Task<bool> DatabaseTableExistsAsync(int databaseTableId, CancellationToken cancellationToken = default);
    Task<bool> DatabaseTableColumnExistsAsync(int databaseTableColumnId, CancellationToken cancellationToken = default);
    Task<bool> BackgroundJobExistsAsync(int backgroundJobId, CancellationToken cancellationToken = default);
    Task<bool> ReportExistsAsync(int reportId, CancellationToken cancellationToken = default);
}
