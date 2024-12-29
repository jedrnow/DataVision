using DataVision.Domain.Enums;

namespace DataVision.Application.Common.Models;
public record ReportChartModel
{
    public string? Title { get; init; }
    public int TableId { get; init; }
    public int LabelColumnId { get; init; }
    public int TargetColumnId { get; init; }
    public ChartType ChartType { get; init; }
}
