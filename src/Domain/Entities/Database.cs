using DataVision.Domain.Enums;

namespace DataVision.Domain.Entities;
public class Database : BaseAuditableEntity
{
    public string? Name { get; set; }
    public string? ConnectionString { get; set; }
    public DatabaseProvider Provider { get; set; }
    public bool IsPopulated { get; set; } = false;
    public IList<DatabaseTable> DatabaseTables { get; private set; } = new List<DatabaseTable>();
    public IList<DatabaseTableColumn> DatabaseTableColumns { get; private set; } = new List<DatabaseTableColumn>();
    public IList<DatabaseTableRow> DatabaseTableRows { get; private set; } = new List<DatabaseTableRow>();
    public IList<DatabaseTableCell> DatabaseTableCells { get; private set; } = new List<DatabaseTableCell>();
    public IList<Report> Reports { get; private set; } = new List<Report>();

    public void Update(string? name, string? connectionString, DatabaseProvider databaseProvider, string? userId = null)
    {
        Name = name;
        ConnectionString = connectionString;
        Provider = databaseProvider;

        Update(userId);
    }
}
