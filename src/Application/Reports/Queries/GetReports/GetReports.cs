using DataVision.Application.Common.Interfaces;
using DataVision.Application.Common.Models;

namespace DataVision.Application.Reports.Queries.GetReports;

public record GetReportsQuery : IRequest<PaginatedList<ReportDto>>
{
    public required PaginatedQuery PaginatedQuery { get; init; }
}

public class GetReportsQueryHandler : IRequestHandler<GetReportsQuery, PaginatedList<ReportDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IUser _user;

    public GetReportsQueryHandler(IApplicationDbContext context, IMapper mapper, IUser user)
    {
        _context = context;
        _mapper = mapper;
        _user = user;
    }

    public async Task<PaginatedList<ReportDto>> Handle(GetReportsQuery request, CancellationToken cancellationToken)
    {
        var userId = _user.Id;

        var reports = _context.Reports
            .AsNoTracking()
            .Where(x => x.CreatedBy == userId)
            .OrderByDescending(x => x.Created)
            .ProjectTo<ReportDto>(_mapper.ConfigurationProvider);

        var paginatedReportsList = await PaginatedList<ReportDto>.CreateAsync(reports, request.PaginatedQuery.PageNumber, request.PaginatedQuery.PageSize);

        return paginatedReportsList;
    }
}
