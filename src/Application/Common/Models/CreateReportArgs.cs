using DataVision.Domain.Enums;

namespace DataVision.Application.Common.Models;
public record CreateReportArgs
{
    public string? Title { get; init; }
    public int DatabaseId { get; init; }
    public ReportFormat? Format { get; init; }
    public List<int> TableIds { get; init; } = [];
    public bool GenerateTables { get; init; }
    public List<ReportChartModel> Charts { get; init; } = [];
}
