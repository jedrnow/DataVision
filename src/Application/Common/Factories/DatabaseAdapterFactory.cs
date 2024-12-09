using System.Text.Json;
using DataVision.Application.Common.Adapters;
using DataVision.Application.Common.Interfaces;
using DataVision.Domain.Enums;

namespace DataVision.Application.Common.Factories;
public static class DatabaseAdapterFactory
{
    public static IDatabaseAdapter CreateAdapter(DatabaseProvider provider, string connectionString)
    {
        return provider switch
        {
            DatabaseProvider.SQLServer => new SqlServerAdapter(connectionString),

            DatabaseProvider.MySQL => new MySqlAdapter(connectionString),

            DatabaseProvider.PostgreSQL => new PostgreSqlAdapter(connectionString),

            _ => throw new NotSupportedException($"Database provider {provider} is not supported.")
        };
    }

    public static IDatabaseAdapter CreateAdapterFromString(string providerString, string connectionString)
    {
        var provider = JsonSerializer.Deserialize<DatabaseProvider>($"\"{providerString}\"");
        return CreateAdapter(provider, connectionString);
    }
}
