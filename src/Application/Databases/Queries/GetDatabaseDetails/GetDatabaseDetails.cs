using DataVision.Application.Common.Interfaces;
using DataVision.Application.Databases.Queries.GetDatabaseDetails;

namespace DataVision.Application.Databases.Queries.GetDatabases;

public record GetDatabaseDetailsQuery : IRequest<DatabaseDetailsDto>
{
    public int DatabaseId { get; set; }
}

public class GetDatabaseDetailsQueryHandler : IRequestHandler<GetDatabaseDetailsQuery, DatabaseDetailsDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDatabaseDetailsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<DatabaseDetailsDto> Handle(GetDatabaseDetailsQuery request, CancellationToken cancellationToken)
    {
        var database = await _context.Databases
            .AsNoTracking()
            .Where(x => x.Id == request.DatabaseId)
            .ProjectTo<DatabaseDetailsDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();

        Guard.Against.NotFound(request.DatabaseId, database);

        return database;
    }
}
