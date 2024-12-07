namespace DataVision.Domain.Entities;
public class Database : BaseAuditableEntity
{
    public string? Name { get; set; }
    public string? ConnectionString { get; set; }
}
