using DataVision.Domain.Enums;

namespace DataVision.Domain.Entities;
public class Report : BaseAuditableEntity
{
    public int DatabaseId { get; set; }
    public Database? Database { get; set; }
    public string? Title { get; set; }
    public ReportFormat Format { get; set; }
    public string? FileName { get; set; }
}
