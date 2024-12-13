using DataVision.Domain.Entities;
using DataVision.Domain.Enums;

namespace DataVision.Application.Databases.Queries.GetDatabases;
public class DatabaseDto
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public DatabaseProvider Provider { get; set; }
    public bool IsPopulated { get; set; } = false;

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Database, DatabaseDto>();
        }
    }
}
