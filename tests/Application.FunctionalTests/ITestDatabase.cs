using System.Data.Common;

namespace DataVision.Application.FunctionalTests;

public interface ITestDatabase
{
    Task InitialiseAsync();

    DbConnection GetConnection();

    Task ResetAsync();

    Task DisposeAsync();
}
