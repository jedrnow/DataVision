using DataVision.Application.Common.Interfaces;

namespace DataVision.Application.Databases.Commands.UpdateDatabase;

public record UpdateDatabaseCommand : IRequest
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? ConnectionString { get; init; }
}

public class UpdateDatabaseCommandHandler : IRequestHandler<UpdateDatabaseCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateDatabaseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateDatabaseCommand request, CancellationToken cancellationToken)
    {
        var database = await _context.Databases.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, database);

        database.Update(request.Name, request.ConnectionString);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
