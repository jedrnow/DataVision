using DataVision.Application.Common.Interfaces;
using DataVision.Application.Common.Models;

namespace DataVision.Application.DatabaseTables.Queries.GetDatabaseTables;

public record GetDatabaseTableRowsQuery : IRequest<PaginatedList<DatabaseTableRowDto>>
{
    public int DatabaseTableId { get; init; }
    public required PaginatedQuery PaginatedQuery { get; init; }
}

public class GetDatabaseTableRowsQueryHandler : IRequestHandler<GetDatabaseTableRowsQuery, PaginatedList<DatabaseTableRowDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDatabaseTableRowsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<DatabaseTableRowDto>> Handle(GetDatabaseTableRowsQuery request, CancellationToken cancellationToken)
    {
        var databaseTableRows = _context.DatabaseTableRows
            .AsNoTracking()
            .Include(x => x.Cells)
            .Where(x => x.DatabaseTableId == request.DatabaseTableId)
            .OrderBy(x => x.Created)
            .ProjectTo<DatabaseTableRowDto>(_mapper.ConfigurationProvider);

        var paginatedDatabaseTableRowsList = await PaginatedList<DatabaseTableRowDto>.CreateAsync(databaseTableRows, request.PaginatedQuery.PageNumber, request.PaginatedQuery.PageSize);

        return paginatedDatabaseTableRowsList;
    }
}
