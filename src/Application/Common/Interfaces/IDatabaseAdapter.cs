using DataVision.Application.Common.Models;
using DataVision.Domain.Enums;

namespace DataVision.Application.Common.Interfaces;
public interface IDatabaseAdapter
{
    Task<bool> CanConnectAsync();
    Task<DatabaseFetchingResult> FetchDatabaseAsync();
    DataType MapColumnType(string? providerSpecificType);
}
