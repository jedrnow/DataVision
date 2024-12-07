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

        _context.Databases.Remove(database);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
