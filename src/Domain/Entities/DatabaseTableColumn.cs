using DataVision.Domain.Enums;

namespace DataVision.Domain.Entities;
public class DatabaseTableColumn : BaseAuditableEntity
{
    public string? Name { get; set; }
    public DataType Type { get; set; }
    public int DatabaseTableId { get; set; }
    public DatabaseTable? DatabaseTable { get; set; }
    public void Update(string? name, DataType type, string? userId = null)
    {
        Name = name;
        Type = type;

        base.Update(userId);
    }
}
