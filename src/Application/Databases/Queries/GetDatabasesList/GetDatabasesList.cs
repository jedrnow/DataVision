using DataVision.Application.Common.Interfaces;
using DataVision.Application.Common.Models;

namespace DataVision.Application.Databases.Queries.GetDatabasesList;

public record GetDatabasesListQuery : IRequest<List<IdNameDto>>
{ }

public class GetDatabasesListQueryHandler : IRequestHandler<GetDatabasesListQuery, List<IdNameDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _user;

    public GetDatabasesListQueryHandler(IApplicationDbContext context, IUser user)
    {
        _context = context;
        _user = user;
    }

    public async Task<List<IdNameDto>> Handle(GetDatabasesListQuery request, CancellationToken cancellationToken)
    {
        var userId = _user.Id;

        var databases = await _context.Databases
            .AsNoTracking()
            .Where(x => x.CreatedBy == userId)
            .OrderBy(x => x.Name)
            .Select(x => new IdNameDto { Id = x.Id, Name = x.Name })
            .ToListAsync(cancellationToken);

        return databases;
    }
}
