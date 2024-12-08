using DataVision.Domain.Entities;
using DataVision.Domain.Enums;

namespace DataVision.Application.DatabaseTables.Queries.GetDatabaseTables;
public class DatabaseTableColumnDto
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public DataType Type { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<DatabaseTableColumn, DatabaseTableColumnDto>();
        }
    }
}
