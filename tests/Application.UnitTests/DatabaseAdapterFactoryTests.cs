using DataVision.Application.Common.Adapters;
using DataVision.Application.Common.Factories;
using DataVision.Domain.Enums;
using FluentAssertions;
using NUnit.Framework;

namespace DataVision.Application.UnitTests;
public class DatabaseAdapterFactoryTests
{
    [Test]
    public void ShouldReturnSqlServerAdapter_WhenProviderIsSqlServer()
    {
        // Arrange
        var provider = DatabaseProvider.SQLServer;
        var connectionString = "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;";

        // Act
        var adapter = DatabaseAdapterFactory.CreateAdapter(provider, connectionString);

        adapter.Should().BeOfType<SqlServerAdapter>();
    }

    [Test]
    public void ShouldReturnMySqlAdapter_WhenProviderIsMySql()
    {
        // Arrange
        var provider = DatabaseProvider.MySQL;
        var connectionString = "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;";

        // Act
        var adapter = DatabaseAdapterFactory.CreateAdapter(provider, connectionString);

        // Assert
        adapter.Should().BeOfType<MySqlAdapter>();
    }

    [Test]
    public void ShouldReturnPostgreSqlAdapter_WhenProviderIsPostgreSql()
    {
        // Arrange
        var provider = DatabaseProvider.PostgreSQL;
        var connectionString = "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;";

        // Act
        var adapter = DatabaseAdapterFactory.CreateAdapter(provider, connectionString);

        // Assert
        adapter.Should().BeOfType<PostgreSqlAdapter>();
    }
}
