namespace DataVision.Application.Common.Models;
public record ReportTableModel
{
    public int TableId { get; init; }
    public List<int> SelectedColumns { get; init; } = [];
}
