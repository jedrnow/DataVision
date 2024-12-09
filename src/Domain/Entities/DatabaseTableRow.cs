namespace DataVision.Domain.Entities;
public class DatabaseTableRow : BaseAuditableEntity
{
    public int DatabaseId { get; set; }
    public Database? Database { get; set; }
    public int DatabaseTableId { get; set; }
    public DatabaseTable? DatabaseTable { get; set; }
    public IList<DatabaseTableCell> Cells { get; private set; } = new List<DatabaseTableCell>();
}
