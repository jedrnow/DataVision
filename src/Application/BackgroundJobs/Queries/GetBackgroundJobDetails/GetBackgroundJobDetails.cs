using DataVision.Application.Common.Interfaces;

namespace DataVision.Application.BackgroundJobs.Queries.GetBackgroundJobDetails;

public record GetBackgroundJobDetailsQuery : IRequest<BackgroundJobDetailsDto>
{
    public int BackgroundJobId { get; set; }
}

public class GetBackgroundJobDetailsQueryHandler : IRequestHandler<GetBackgroundJobDetailsQuery, BackgroundJobDetailsDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetBackgroundJobDetailsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BackgroundJobDetailsDto> Handle(GetBackgroundJobDetailsQuery request, CancellationToken cancellationToken)
    {
        var job = await _context.BackgroundJobs
            .AsNoTracking()
            .Where(x => x.Id == request.BackgroundJobId)
            .ProjectTo<BackgroundJobDetailsDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();

        Guard.Against.NotFound(request.BackgroundJobId, job);

        return job;
    }
}
