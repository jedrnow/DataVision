using DataVision.Application.DatabaseTables.Queries.GetDatabaseTables;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DataVision.Web.Endpoints;

public class DatabaseTableColumns : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetDatabaseTableColumns);
    }

    public async Task<Ok<List<DatabaseTableColumnDto>>> GetDatabaseTableColumns(ISender sender, int databaseTableId)
    {
        var query = new GetDatabaseTableColumnsQuery()
        {
            DatabaseTableId = databaseTableId
        };

        var result = await sender.Send(query);

        return TypedResults.Ok(result);
    }
}
