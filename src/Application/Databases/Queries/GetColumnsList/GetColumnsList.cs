using DataVision.Application.Common.Interfaces;
using DataVision.Application.Common.Models;
using DataVision.Domain.Enums;

namespace DataVision.Application.Databases.Queries.GetColumnsList;

public record GetColumnsListQuery : IRequest<List<IdNameDto>>
{
    public int DatabaseId { get; init; }
    public int DatabaseTableId { get; init; }
    public bool OnlyNumeric { get; init; }
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
            .Select(x => new { Id = x.Id, Name = x.Name, Type = x.Type })
            .ToListAsync(cancellationToken);

        if (request.OnlyNumeric)
        {
            columns = columns.Where(x => IsNumeric(x.Type)).ToList();
        }

        return columns.Select(x => new IdNameDto { Id = x.Id, Name = x.Name }).ToList();
    }

    private static bool IsNumeric(DataType type)
    {
        return type switch
        {
            DataType.Long => true,
            DataType.Decimal => true,
            DataType.Float => true,
            DataType.Int => true,
            DataType.Double => true,
            _ => false
        };
    }
}
