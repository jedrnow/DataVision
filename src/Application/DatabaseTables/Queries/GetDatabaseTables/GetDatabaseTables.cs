using DataVision.Application.Common.Interfaces;
using DataVision.Application.Common.Models;

namespace DataVision.Application.DatabaseTables.Queries.GetDatabaseTables;

public record GetDatabaseTablesQuery : IRequest<PaginatedList<DatabaseTableDto>>
{
    public int DatabaseId { get; init; }
    public required PaginatedQuery PaginatedQuery { get; init; }
}

public class GetDatabaseTablesQueryHandler : IRequestHandler<GetDatabaseTablesQuery, PaginatedList<DatabaseTableDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDatabaseTablesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<DatabaseTableDto>> Handle(GetDatabaseTablesQuery request, CancellationToken cancellationToken)
    {
        var databaseTables = _context.DatabaseTables
            .AsNoTracking()
            .Where(x => x.DatabaseId == request.DatabaseId)
            .OrderBy(x => x.Created)
            .ProjectTo<DatabaseTableDto>(_mapper.ConfigurationProvider);

        var paginatedDatabaseTablesList = await PaginatedList<DatabaseTableDto>.CreateAsync(databaseTables, request.PaginatedQuery.PageNumber, request.PaginatedQuery.PageSize);

        return paginatedDatabaseTablesList;
    }
}
