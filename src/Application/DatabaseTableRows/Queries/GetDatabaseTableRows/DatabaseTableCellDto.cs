using System.ComponentModel.DataAnnotations;
using DataVision.Domain.Entities;

namespace DataVision.Application.DatabaseTableRows.Queries.GetDatabaseTableRows;
public class DatabaseTableCellDto
{
    public int Id { get; init; }
    public DataType Type { get; init; }
    public string? Value { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<DatabaseTableCell, DatabaseTableCellDto>();
        }
    }
}
