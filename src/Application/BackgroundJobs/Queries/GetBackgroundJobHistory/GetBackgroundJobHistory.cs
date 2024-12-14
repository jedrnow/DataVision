using DataVision.Application.BackgroundJobs.Queries.GetBackgroundJobDetails;
using DataVision.Application.Common.Interfaces;
using DataVision.Application.Common.Models;

namespace DataVision.Application.BackgroundJobs.Queries.GetBackgroundJobHistory;

public record GetBackgroundJobHistoryQuery : IRequest<PaginatedList<BackgroundJobDetailsDto>>
{
    public int DatabaseId { get; set; }
    public required PaginatedQuery PaginatedQuery { get; init; }
}

public class GetBackgroundJobHistoryQueryHandler : IRequestHandler<GetBackgroundJobHistoryQuery, PaginatedList<BackgroundJobDetailsDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetBackgroundJobHistoryQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<BackgroundJobDetailsDto>> Handle(GetBackgroundJobHistoryQuery request, CancellationToken cancellationToken)
    {
        var jobs = _context.BackgroundJobs
            .AsNoTracking()
            .Where(x => x.DatabaseId == request.DatabaseId)
            .ProjectTo<BackgroundJobDetailsDto>(_mapper.ConfigurationProvider);

        var paginatedJobList = await PaginatedList<BackgroundJobDetailsDto>.CreateAsync(jobs, request.PaginatedQuery.PageNumber, request.PaginatedQuery.PageSize);

        return paginatedJobList;
    }
}
