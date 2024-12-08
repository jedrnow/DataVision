namespace DataVision.Domain.Entities;
public class DatabaseTableRow : BaseAuditableEntity
{
    public int DatabaseTableId { get; set; }
    public DatabaseTable? DatabaseTable { get; set; }
    public IList<DatabaseTableCell> Cells { get; private set; } = new List<DatabaseTableCell>();
}
