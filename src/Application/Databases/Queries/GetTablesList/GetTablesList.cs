using DataVision.Application.Common.Interfaces;
using DataVision.Application.Common.Models;

namespace DataVision.Application.Databases.Queries.GetTablesList;

public record GetTablesListQuery : IRequest<List<IdNameDto>>
{
    public int DatabaseId { get; init; }
}

public class GetTablesListQueryHandler : IRequestHandler<GetTablesListQuery, List<IdNameDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _user;

    public GetTablesListQueryHandler(IApplicationDbContext context, IUser user)
    {
        _context = context;
        _user = user;
    }

    public async Task<List<IdNameDto>> Handle(GetTablesListQuery request, CancellationToken cancellationToken)
    {
        var userId = _user.Id;

        var tables = await _context.DatabaseTables
            .AsNoTracking()
            .Where(x => x.CreatedBy == userId && x.DatabaseId == request.DatabaseId)
            .OrderBy(x => x.Name)
            .Select(x => new IdNameDto { Id = x.Id, Name = x.Name })
            .ToListAsync(cancellationToken);

        return tables;
    }
}
