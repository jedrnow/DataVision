namespace DataVision.Domain.Entities;
public class Database : BaseAuditableEntity
{
    public string? Name { get; set; }
    public string? ConnectionString { get; set; }
    public IList<DatabaseTable> DatabaseTables { get; private set; } = new List<DatabaseTable>();

    public void Update(string? name, string? connectionString, string? userId = null)
    {
        Name = name;
        ConnectionString = connectionString;

        Update(userId);
    }
}
