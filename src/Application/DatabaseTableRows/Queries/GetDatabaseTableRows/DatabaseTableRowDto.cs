using DataVision.Domain.Entities;

namespace DataVision.Application.DatabaseTables.Queries.GetDatabaseTables;
public class DatabaseTableRowDto
{
    public int Id { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<DatabaseTableRow, DatabaseTableRowDto>();
        }
    }
}
