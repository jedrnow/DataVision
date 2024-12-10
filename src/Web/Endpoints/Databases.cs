using DataVision.Application.Common.Models;
using DataVision.Application.Databases.Commands.CreateDatabase;
using DataVision.Application.Databases.Commands.DeleteDatabase;
using DataVision.Application.Databases.Commands.PopulateDatabase;
using DataVision.Application.Databases.Commands.UpdateDatabase;
using DataVision.Application.Databases.Queries.GetDatabases;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DataVision.Web.Endpoints;

public class Databases : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetDatabases)
            .MapPost(CreateDatabase)
            .MapPost(PopulateDatabase, "{id}/Populate")
            .MapPut(UpdateDatabase, "{id}")
            .MapDelete(DeleteDatabase, "{id}");
    }

    public async Task<Ok<PaginatedList<DatabaseDto>>> GetDatabases(ISender sender, int pageNumber, int pageSize)
    {
        var query = new GetDatabasesQuery()
        {
            PaginatedQuery = new PaginatedQuery()
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            }
        };

        var result = await sender.Send(query);

        return TypedResults.Ok(result);
    }

    public async Task<Created<int>> CreateDatabase(ISender sender, CreateDatabaseCommand command)
    {
        var id = await sender.Send(command);

        return TypedResults.Created($"/{nameof(Databases)}/{id}", id);
    }

    public async Task<Results<NoContent, BadRequest>> UpdateDatabase(ISender sender, int id, UpdateDatabaseCommand command)
    {
        if (id != command.Id) return TypedResults.BadRequest();

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    public async Task<Ok<int>> DeleteDatabase(ISender sender, int id)
    {
        var command = new DeleteDatabaseCommand() { Id = id };

        var result = await sender.Send(command);

        return TypedResults.Ok(result);
    }

    public async Task<Ok<int>> PopulateDatabase(ISender sender, int id)
    {
        var command = new PopulateDatabaseCommand() { DatabaseId = id };

        var result = await sender.Send(command);

        return TypedResults.Ok(result);
    }
}
