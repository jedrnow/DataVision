using DataVision.Application.Common.Interfaces;
using DataVision.Application.Common.Models;
using DataVision.Domain.Enums;
using Npgsql;

namespace DataVision.Application.Common.Adapters;
public class PostgreSqlAdapter : IDatabaseAdapter
{
    private readonly string _connectionString;

    public PostgreSqlAdapter(string connectionString)
    {
        _connectionString = connectionString;
    }
    public async Task<bool> CanConnectAsync()
    {
        try
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<DatabaseFetchingResult> FetchDatabaseAsync()
    {
        var result = new DatabaseFetchingResult();
        var tables = new List<FetchedTable>();

        try
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT table_name FROM information_schema.tables WHERE table_schema = 'public'";
            using var command = new NpgsqlCommand(query, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var tableName = reader.GetString(0);
                var table = new FetchedTable { Name = tableName };

                table.Columns = await FetchColumnsAsync(connection, tableName);
                table.Rows = await FetchRowsAsync(connection, tableName);

                tables.Add(table);
            }
        }
        catch(Exception ex)
        {
            result.Errors.Add(ex.Message);
            result.Success = false;
        }

        result.Tables = tables;
        return result;
    }

    private async Task<List<FetchedColumn>> FetchColumnsAsync(NpgsqlConnection connection, string tableName)
    {
        var columns = new List<FetchedColumn>();

        var query = $"SELECT column_name, data_type FROM information_schema.columns WHERE table_name = '{tableName}' AND table_schema = 'public'";
        using var command = new NpgsqlCommand(query, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            columns.Add(new FetchedColumn
            {
                Name = reader.GetString(0),
                DataType = MapColumnType(reader.GetString(1))
            });
        }

        return columns;
    }

    private static async Task<List<FetchedRow>> FetchRowsAsync(NpgsqlConnection connection, string tableName)
    {
        var rows = new List<FetchedRow>();
        var query = $"SELECT * FROM \"{tableName}\"";

        using var command = new NpgsqlCommand(query, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var row = new FetchedRow();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                var cell = new FetchedCell
                {
                    ColumnName = reader.GetName(i),
                    Value = reader.IsDBNull(i) ? null : reader.GetValue(i)
                };
                row.Cells.Add(cell);
            }

            rows.Add(row);
        }

        return rows;
    }

    public DataType MapColumnType(string? providerSpecificType)
    {
        if (providerSpecificType == null)
        {
            return DataType.Unknown;
        }

        return providerSpecificType.ToLower() switch
        {
            "integer" => DataType.Int,
            "smallint" => DataType.Int,
            "bigint" => DataType.Long,
            "decimal" => DataType.Decimal,
            "numeric" => DataType.Decimal,
            "real" => DataType.Float,
            "double precision" => DataType.Double,
            "boolean" => DataType.Bool,
            "timestamp without time zone" => DataType.DateTime,
            "timestamp with time zone" => DataType.DateTime,
            "date" => DataType.Date,
            "time without time zone" => DataType.Time,
            "time with time zone" => DataType.Time,
            "interval" => DataType.TimeSpan,
            "character varying" => DataType.String,
            "character" => DataType.String,
            "text" => DataType.String,
            "uuid" => DataType.Guid,
            "bytea" => DataType.Binary,
            _ => DataType.Unknown
        };
    }
}

