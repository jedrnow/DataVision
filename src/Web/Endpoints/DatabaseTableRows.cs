using DataVision.Application.Common.Models;
using DataVision.Application.DatabaseTables.Queries.GetDatabaseTables;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DataVision.Web.Endpoints;

public class DatabaseTableRows : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetDatabaseTableRows);
    }

    public async Task<Ok<PaginatedList<DatabaseTableRowDto>>> GetDatabaseTableRows(ISender sender, int databaseTableId, int pageNumber, int pageSize)
    {
        var query = new GetDatabaseTableRowsQuery()
        {
            DatabaseTableId = databaseTableId,
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
