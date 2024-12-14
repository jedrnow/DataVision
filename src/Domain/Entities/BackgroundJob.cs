using DataVision.Domain.Enums;

namespace DataVision.Domain.Entities;
public class BackgroundJob : BaseAuditableEntity
{
    public string? ExternalJobId { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsSucceeded { get; set; }
    public string? Message { get; set; }
    public string? Result { get; set; }
    public int? DatabaseId { get; set; }
    public BackgroundJobType Type { get; set; }
}
