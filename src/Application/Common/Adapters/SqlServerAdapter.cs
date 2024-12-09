using System.Data;
using DataVision.Application.Common.Interfaces;
using DataVision.Application.Common.Models;
using DataVision.Domain.Enums;
using Microsoft.Data.SqlClient;
using MySqlX.XDevAPI;

namespace DataVision.Application.Common.Adapters;

public class SqlServerAdapter : IDatabaseAdapter
{
    private readonly string _connectionString;

    public SqlServerAdapter(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<bool> CanConnectAsync()
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
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
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var queryTables = @"
                SELECT TABLE_SCHEMA, TABLE_NAME
                FROM INFORMATION_SCHEMA.TABLES
                WHERE TABLE_TYPE = 'BASE TABLE'";

            using var commandTables = new SqlCommand(queryTables, connection);
            using var readerTables = await commandTables.ExecuteReaderAsync();

            while (await readerTables.ReadAsync())
            {
                var schema = readerTables.GetString(0);
                var tableName = readerTables.GetString(1);

                var table = new FetchedTable { Name = tableName, Schema = schema };
                tables.Add(table);
            }

            await readerTables.CloseAsync();

            foreach(var table in tables)
            {
                if(table.Schema != null && table.Name != null)
                {
                    table.Columns = await FetchColumns(connection, table.Schema, table.Name);
                    table.Rows = await FetchRowsAsync(connection, table.Schema, table.Name, table.Columns);
                }
                else
                {
                    result.Errors.Add("Null schema or null table name found.");
                }
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

    private async Task<List<FetchedColumn>> FetchColumns(SqlConnection connection, string schema, string tableName)
    {
        var columns = new List<FetchedColumn>();
        var queryColumns = @"
            SELECT COLUMN_NAME, DATA_TYPE
            FROM INFORMATION_SCHEMA.COLUMNS
            WHERE TABLE_SCHEMA = @Schema AND TABLE_NAME = @TableName";

        using var commandColumns = new SqlCommand(queryColumns, connection);
        commandColumns.Parameters.AddWithValue("@Schema", schema);
        commandColumns.Parameters.AddWithValue("@TableName", tableName);

        using var readerColumns = await commandColumns.ExecuteReaderAsync();
        while (await readerColumns.ReadAsync())
        {
            columns.Add(new FetchedColumn
            {
                Name = readerColumns.GetString(0),
                DataType = MapColumnType(readerColumns.GetString(1)),
            });
        }

        return columns;
    }

    private static async Task<List<FetchedRow>> FetchRowsAsync(SqlConnection connection, string schema, string tableName, List<FetchedColumn> columns)
    {
        var rows = new List<FetchedRow>();
        var query = $"SELECT * FROM [{schema}].[{tableName}]";

        using var command = new SqlCommand(query, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var row = new FetchedRow();
            foreach (var column in columns)
            {
                row.Cells.Add(new FetchedCell
                {
                    ColumnName = column.Name,
                    Value = reader[column.Name]
                });
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
            "smallint" => DataType.Int,
            "bigint" => DataType.Long,
            "tinyint" => DataType.Int,
            "decimal" => DataType.Decimal,
            "numeric" => DataType.Decimal,
            "float" => DataType.Float,
            "real" => DataType.Double,
            "bit" => DataType.Bool,
            "datetime" => DataType.DateTime,
            "datetime2" => DataType.DateTime,
            "smalldatetime" => DataType.DateTime,
            "date" => DataType.Date,
            "time" => DataType.Time,
            "uniqueidentifier" => DataType.Guid,
            "char" => DataType.String,
            "varchar" => DataType.String,
            "text" => DataType.String,
            "nchar" => DataType.String,
            "nvarchar" => DataType.String,
            "ntext" => DataType.String,
            "binary" => DataType.Binary,
            "varbinary" => DataType.Binary,
            "image" => DataType.Binary,
            _ => DataType.Unknown
        };
    }
}
