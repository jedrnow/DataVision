using DataVision.Domain.Enums;

namespace DataVision.Domain.Entities;
public class DatabaseTableCell : BaseAuditableEntity
{
    public int DatabaseId { get; set; }
    public Database? Database { get; set; }
    public int DatabaseTableId { get; set; }
    public DatabaseTable? DatabaseTable { get; set; }
    public int DatabaseTableRowId { get; set; }
    public DatabaseTableRow? DatabaseTableRow { get; set; }
    public int DatabaseTableColumnId { get; set; }
    public DatabaseTableColumn? DatabaseTableColumn { get; set; }
    public DataType Type { get; set; }
    public string? Value { get; set; }

    public void Update(string? value, DataType type, string? userId = null)
    {
        Value = value;
        Type = type;

        Update(userId);
    }
}
