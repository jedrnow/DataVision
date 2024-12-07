using DataVision.Domain.Entities;

namespace DataVision.Application.Databases.Queries.GetDatabases;
public class DatabaseDto
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? ConnectionString { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Database, DatabaseDto>();
        }
    }
}
