using DataVision.Domain.Entities;

namespace DataVision.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Database> Databases { get; }
    DbSet<DatabaseTable> DatabaseTables { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
