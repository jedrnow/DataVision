namespace DataVision.Domain.Entities;
public class DatabaseTable : BaseAuditableEntity
{
    public string? Name { get; set; }
    public int DatabaseId { get; set; }
    public Database? Database { get; set; }
    public void Update(string? name, string? userId = null)
    {
        Name = name;

        base.Update(userId);
    }
}
