using DataVision.Application.Common.Interfaces;
using DataVision.Application.Common.Jobs;
using DataVision.Application.Common.Models;
using DataVision.Domain.Entities;
using DataVision.Domain.Enums;

namespace DataVision.Application.Reports.Commands.CreateReport;

public record CreateReportCommand : IRequest<int>
{
    public int DatabaseId { get; init; }
    public string? Title { get; init; }
    public List<ReportTableModel> Tables { get; init; } = [];
    public ReportFormat? Format { get; init; }
    public bool GenerateTables { get; init; }
    public List<ReportChartModel> Charts { get; init; } = [];
}

public class CreateReportCommandHandler : IRequestHandler<CreateReportCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _user;

    public CreateReportCommandHandler(IApplicationDbContext context, IUser user)
    {
        _context = context;
        _user = user;
    }

    public async Task<int> Handle(CreateReportCommand request, CancellationToken cancellationToken)
    {
        var userId = _user.Id;
        var database = await _context.Databases.SingleOrDefaultAsync(x => x.Id == request.DatabaseId, cancellationToken);

        Guard.Against.NotFound(request.DatabaseId, database);
        Guard.Against.NullOrEmpty(request.Title, parameterName: nameof(request.Title));
        Guard.Against.Null(request.Format, parameterName: nameof(request.Format));

        var job = new BackgroundJob();
        _context.BackgroundJobs.Add(job);
        await _context.SaveChangesAsync(cancellationToken);

        var jobArgs = new CreateReportArgs
        {
            DatabaseId = request.DatabaseId,
            Title = request.Title,
            Format = request.Format.Value,
            Tables = request.Tables,
            GenerateTables = request.GenerateTables,
            Charts = request.Charts
        };
        var externalJobId = Hangfire.BackgroundJob.Enqueue<CreateReportJob>(x => x.Run(job.Id, userId, jobArgs, cancellationToken));

        job.ExternalJobId = externalJobId;
        _context.BackgroundJobs.Update(job);
        await _context.SaveChangesAsync(cancellationToken);

        return job.Id;
    }
}
