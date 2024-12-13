using DataVision.Domain.Entities;
using DataVision.Domain.Enums;

namespace DataVision.Application.Databases.Queries.GetDatabaseDetails;
public class DatabaseDetailsDto
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public DatabaseProvider Provider { get; set; }
    public bool IsPopulated { get; set; } = false;
    public string? ConnectionString { get; set; }
    public int TablesCount { get; set; }
    public int RowsCount { get; set; }
    public int CellsCount { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Database, DatabaseDetailsDto>()
                .ForMember(dest => dest.TablesCount, opt => opt.MapFrom(x => x.DatabaseTables.Count))
                .ForMember(dest => dest.RowsCount, opt => opt.MapFrom(x => x.DatabaseTableRows.Count))
                .ForMember(dest => dest.CellsCount, opt => opt.MapFrom(x => x.DatabaseTableCells.Count));
        }
    }
}
