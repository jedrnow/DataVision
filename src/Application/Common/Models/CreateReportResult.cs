using DataVision.Domain.Enums;

namespace DataVision.Application.Common.Models;
public record CreateReportResult
{
    public bool Success { get; set; } = false;
    public string? Title { get; set; }
    public string? FileName { get; set; }
    public ReportFormat? Format { get; set; }
    public List<int> TableIds { get; set; } = new List<int>();
    public string? Message { get; set; }
}
