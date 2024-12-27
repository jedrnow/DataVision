using DataVision.Application.Common.Interfaces;
using DataVision.Application.Common.Models;

namespace DataVision.Application.Databases.Queries.GetDatabases;

public record GetDatabasesQuery : IRequest<PaginatedList<DatabaseDto>>
{
    public required PaginatedQuery PaginatedQuery { get; init; }
}

public class GetDatabasesQueryHandler : IRequestHandler<GetDatabasesQuery, PaginatedList<DatabaseDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDatabasesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<DatabaseDto>> Handle(GetDatabasesQuery request, CancellationToken cancellationToken)
    {
        var databases = _context.Databases
            .AsNoTracking()
            .OrderByDescending(x => x.Created)
            .ProjectTo<DatabaseDto>(_mapper.ConfigurationProvider);

        var paginatedDatabasesList = await PaginatedList<DatabaseDto>.CreateAsync(databases, request.PaginatedQuery.PageNumber, request.PaginatedQuery.PageSize);

        return paginatedDatabasesList;
    }
}
