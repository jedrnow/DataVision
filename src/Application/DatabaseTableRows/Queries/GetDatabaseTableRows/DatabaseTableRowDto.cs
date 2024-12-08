using DataVision.Application.DatabaseTableRows.Queries.GetDatabaseTableRows;
using DataVision.Domain.Entities;

namespace DataVision.Application.DatabaseTables.Queries.GetDatabaseTables;
public class DatabaseTableRowDto
{
    public int Id { get; init; }
    public List<DatabaseTableCellDto> Cells { get; init; } = [];

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<DatabaseTableRow, DatabaseTableRowDto>()
                .ForMember(x => x.Cells, x => x.MapFrom(t => t.Cells));
        }
    }
}
