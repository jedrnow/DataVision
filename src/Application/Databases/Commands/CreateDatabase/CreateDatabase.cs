using DataVision.Application.Common.Interfaces;
using DataVision.Domain.Entities;
using DataVision.Domain.Enums;

namespace DataVision.Application.Databases.Commands.CreateDatabase;

public record CreateDatabaseCommand : IRequest<int>
{
    public string? Name { get; init; }
    public string? ConnectionString { get; init; }
    public DatabaseProvider DatabaseProvider { get; init; }
}

public class CreateDatabaseCommandHandler : IRequestHandler<CreateDatabaseCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateDatabaseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateDatabaseCommand request, CancellationToken cancellationToken)
    {
        var database = new Database()
        {
            Name = request.Name,
            ConnectionString = request.ConnectionString,
            Provider = request.DatabaseProvider,
        };

        _context.Databases.Add(database);

        await _context.SaveChangesAsync(cancellationToken);

        return database.Id;
    }
}
