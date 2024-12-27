using DataVision.Application.Common.Interfaces;
using DataVision.Application.Common.Models;

namespace DataVision.Application.Databases.Queries.GetColumnsList;

public record GetColumnsListQuery : IRequest<List<IdNameDto>>
{
    public int DatabaseId { get; init; }
    public int DatabaseTableId { get; init; }
}

public class GetColumnsListQueryHandler : IRequestHandler<GetColumnsListQuery, List<IdNameDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _user;

    public GetColumnsListQueryHandler(IApplicationDbContext context, IUser user)
    {
        _context = context;
        _user = user;
    }

    public async Task<List<IdNameDto>> Handle(GetColumnsListQuery request, CancellationToken cancellationToken)
    {
        var userId = _user.Id;

        var columns = await _context.DatabaseTableColumns
            .AsNoTracking()
            .Where(x => x.CreatedBy == userId && x.DatabaseId == request.DatabaseId && x.DatabaseTableId == request.DatabaseTableId)
            .OrderBy(x => x.Name)
            .Select(x => new IdNameDto { Id = x.Id, Name = x.Name })
            .ToListAsync(cancellationToken);

        return columns;
    }
}
