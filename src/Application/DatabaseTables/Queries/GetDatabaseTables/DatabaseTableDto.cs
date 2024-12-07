using DataVision.Domain.Entities;

namespace DataVision.Application.DatabaseTables.Queries.GetDatabaseTables;
public class DatabaseTableDto
{
    public int Id { get; init; }
    public string? Name { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<DatabaseTable, DatabaseTableDto>();
        }
    }
}
