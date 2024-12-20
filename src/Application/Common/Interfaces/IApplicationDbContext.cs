using DataVision.Domain.Entities;

namespace DataVision.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Database> Databases { get; }
    DbSet<DatabaseTable> DatabaseTables { get; }
    DbSet<DatabaseTableColumn> DatabaseTableColumns { get; }
    DbSet<DatabaseTableRow> DatabaseTableRows {  get; }
    DbSet<DatabaseTableCell> DatabaseTableCells { get; }
    DbSet<BackgroundJob> BackgroundJobs { get; }
    DbSet<Report> Reports { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
