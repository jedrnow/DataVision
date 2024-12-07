using DataVision.Domain.Entities;

namespace DataVision.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }

    DbSet<Database> Databases { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
