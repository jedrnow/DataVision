namespace DataVision.Domain.Entities;
public class DatabaseTableRow : BaseAuditableEntity
{
    public int DatabaseTableId { get; set; }
    public DatabaseTable? DatabaseTable { get; set; }
}
