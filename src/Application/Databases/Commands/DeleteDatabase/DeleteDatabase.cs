using DataVision.Application.Common.Interfaces;

namespace DataVision.Application.Databases.Commands.DeleteDatabase;

public record DeleteDatabaseCommand : IRequest
{
    public int Id { get; init; }
}

public class DeleteDatabaseCommandHandler : IRequestHandler<DeleteDatabaseCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteDatabaseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteDatabaseCommand request, CancellationToken cancellationToken)
    {
        var database = await _context.Databases.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, database);

        var tables = await _context.DatabaseTables
            .Include(x => x.Columns)
            .Include(x => x.Cells)
            .Where(x => x.DatabaseId == database.Id)
            .ToListAsync(cancellationToken);

        var columns = tables.SelectMany(x => x.Columns);

        var cells = tables.SelectMany(x => x.Cells);

        _context.Databases.Remove(database);
        _context.DatabaseTables.RemoveRange(tables);
        _context.DatabaseTableColumns.RemoveRange(columns);
        _context.DatabaseTableCells.RemoveRange(cells);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
