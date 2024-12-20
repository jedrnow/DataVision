using DataVision.Application.Common.Interfaces;

namespace DataVision.Application.Reports.Commands.DeleteReport;

public record DeleteReportCommand : IRequest<bool>
{
    public int Id { get; init; }
}

public class DeleteReportCommandHandler : IRequestHandler<DeleteReportCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteReportCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteReportCommand request, CancellationToken cancellationToken)
    {
        var report = await _context.Reports.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, report);

        _context.Reports.Remove(report);

        return (await _context.SaveChangesAsync(cancellationToken) > 0);
    }
}
