using DataVision.Application.Common.Models;
using DataVision.Application.DatabaseTables.Queries.GetDatabaseTables;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DataVision.Web.Endpoints;

public class DatabaseTables : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetDatabaseTables);
    }

    public async Task<Ok<PaginatedList<DatabaseTableDto>>> GetDatabaseTables(ISender sender, int databaseId, int pageNumber, int pageSize)
    {
        var query = new GetDatabaseTablesQuery()
        {
            DatabaseId = databaseId,
            PaginatedQuery = new PaginatedQuery()
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            }
        };

        var result = await sender.Send(query);

        return TypedResults.Ok(result);
    }
}
