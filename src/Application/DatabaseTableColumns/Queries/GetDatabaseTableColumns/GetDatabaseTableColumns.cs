using DataVision.Application.Common.Interfaces;

namespace DataVision.Application.DatabaseTables.Queries.GetDatabaseTables;

public record GetDatabaseTableColumnsQuery : IRequest<List<DatabaseTableColumnDto>>
{
    public int DatabaseTableId { get; init; }
}

public class GetDatabaseTableColumnsQueryHandler : IRequestHandler<GetDatabaseTableColumnsQuery, List<DatabaseTableColumnDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDatabaseTableColumnsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<DatabaseTableColumnDto>> Handle(GetDatabaseTableColumnsQuery request, CancellationToken cancellationToken)
    {
        var databaseTableColumns = await _context.DatabaseTableColumns
            .AsNoTracking()
            .Where(x => x.DatabaseTableId == request.DatabaseTableId)
            .OrderBy(x => x.Created)
            .ProjectTo<DatabaseTableColumnDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return databaseTableColumns;
    }
}
