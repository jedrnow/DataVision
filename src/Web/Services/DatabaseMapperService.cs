using DataVision.Application.Common.Interfaces;
using DataVision.Application.Common.Models;
using DataVision.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataVision.Web.Services;

public class DatabaseMapperService : IDatabaseMapperService
{
    private readonly IApplicationDbContext _context;

    public DatabaseMapperService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DatabaseMappingResult> MapDatabase(int databaseId, List<FetchedTable> fetchedTables, CancellationToken cancellationToken = default)
    {
        var result = new DatabaseMappingResult();

        var database = await _context.Databases.AsNoTracking().SingleOrDefaultAsync(x => x.Id == databaseId);
        if (database == null)
        {
            result.Success = false;
            result.Errors.Add("Database by id =" + databaseId + " not found.");
            return result;
        }

        result.DatabaseName = database.Name ?? "";

        try
        {
            foreach (var fetchedTable in fetchedTables)
            {
                var newTable = new DatabaseTable()
                {
                    Name = fetchedTable.Name,
                    DatabaseId = databaseId,
                };
                _context.DatabaseTables.Add(newTable);
                result.TablesTotal++;
                result.TableNames.Add(fetchedTable.Name ?? "");

                var newColumns = fetchedTable.Columns.Select(c => new DatabaseTableColumn()
                {
                    Name = c.Name,
                    DatabaseTable = newTable,
                    Type = c.DataType
                });
                _context.DatabaseTableColumns.AddRange(newColumns);

                foreach (var row in fetchedTable.Rows)
                {
                    var newRow = new DatabaseTableRow()
                    {
                        DatabaseTable = newTable,
                    };
                    _context.DatabaseTableRows.Add(newRow);

                    var newCellsDict = row.Cells.Select(c => (Cell: c, Column: newColumns.FirstOrDefault(col => col.Name == c.ColumnName)));

                    foreach (var c in newCellsDict)
                    {
                        if (c.Column is null)
                        {
                            result.Errors.Add("[Table: " + newTable.Name + "] Column not found: " + c.Cell.ColumnName);
                            continue;
                        }

                        var newCell = new DatabaseTableCell()
                        {
                            DatabaseTable = newTable,
                            DatabaseTableRow = newRow,
                            DatabaseTableColumn = c.Column,
                            Type = c.Column.Type,
                            Value = c.Cell.Value?.ToString(),
                        };
                        _context.DatabaseTableCells.Add(newCell);
                    }
                }

                await _context.SaveChangesAsync(cancellationToken);
            }

        }
        catch (Exception ex)
        {
            result.Errors.Add(ex.Message);
            result.Success = false;
        }

        return result;
    }
}
