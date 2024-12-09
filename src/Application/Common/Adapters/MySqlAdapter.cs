using DataVision.Application.Common.Interfaces;
using DataVision.Application.Common.Models;
using DataVision.Domain.Enums;
using MySql.Data.MySqlClient;

namespace DataVision.Application.Common.Adapters;

public class MySqlAdapter : IDatabaseAdapter
{
    private readonly string _connectionString;

    public MySqlAdapter(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<bool> CanConnectAsync()
    {
        try
        {
            using var connection = new MySqlConnection(_connectionString);
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
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = DATABASE()";
            using var command = new MySqlCommand(query, connection);
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
        catch (Exception ex)
        {
            result.Errors.Add(ex.Message);
            result.Success = false;
        }

        result.Tables = tables;
        return result;
    }

    private async Task<List<FetchedColumn>> FetchColumnsAsync(MySqlConnection connection, string tableName)
    {
        var columns = new List<FetchedColumn>();

        var query = $"SELECT COLUMN_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}' AND TABLE_SCHEMA = DATABASE()";
        using var command = new MySqlCommand(query, connection);
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

    private static async Task<List<FetchedRow>> FetchRowsAsync(MySqlConnection connection, string tableName)
    {
        var rows = new List<FetchedRow>();
        var query = $"SELECT * FROM `{tableName}`";

        using var command = new MySqlCommand(query, connection);
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
            "int" => DataType.Int,
            "tinyint" => DataType.Bool,
            "smallint" => DataType.Int,
            "mediumint" => DataType.Int,
            "bigint" => DataType.Long,
            "decimal" => DataType.Decimal,
            "numeric" => DataType.Decimal,
            "float" => DataType.Float,
            "double" => DataType.Double,
            "bit" => DataType.Bool,
            "datetime" => DataType.DateTime,
            "timestamp" => DataType.DateTime,
            "date" => DataType.Date,
            "time" => DataType.Time,
            "char" => DataType.String,
            "varchar" => DataType.String,
            "text" => DataType.String,
            "longtext" => DataType.String,
            "binary" => DataType.Binary,
            "varbinary" => DataType.Binary,
            "blob" => DataType.Binary,
            "uuid" => DataType.Guid,
            _ => DataType.Unknown
        };
    }
}
