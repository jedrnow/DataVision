using DataVision.Domain.Enums;

namespace DataVision.Application.Common.Models;

public class DatabaseFetchingResult
{
    public bool Success { get; set; } = true;
    public List<FetchedTable> Tables { get; set; } = [];
    public List<string> Errors { get; set; } = [];
}

public record FetchedTable
{
    public string? Name { get; set; }
    public string? Schema { get; set; }
    public List<FetchedColumn> Columns { get; set; } = [];
    public List<FetchedRow> Rows { get; set; } = [];
}

public record FetchedColumn
{
    public string? Name { get; set; }
    public DataType DataType { get; set; }
}

public record FetchedRow
{
    public List<FetchedCell> Cells { get; set; } = [];
}

public record FetchedCell
{
    public string? ColumnName { get; set; }
    public object? Value { get; set; }
}
