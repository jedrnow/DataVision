﻿using DataVision.Application.BackgroundJobs.Queries.GetBackgroundJobDetails;
using DataVision.Application.Common.Models;
using DataVision.Application.Databases.Commands.ClearDatabase;
using DataVision.Application.Databases.Commands.CreateDatabase;
using DataVision.Application.Databases.Commands.DeleteDatabase;
using DataVision.Application.Databases.Commands.PopulateDatabase;
using DataVision.Application.Databases.Commands.UpdateDatabase;
using DataVision.Application.Databases.Queries.GetColumnsList;
using DataVision.Application.Databases.Queries.GetDatabaseDetails;
using DataVision.Application.Databases.Queries.GetDatabases;
using DataVision.Application.Databases.Queries.GetDatabasesList;
using DataVision.Application.Databases.Queries.GetTablesList;
using DataVision.Application.Databases.Queries.TestConnection;
using DataVision.Domain.Enums;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DataVision.Web.Endpoints;

public class Databases : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetDatabases)
            .MapGet(GetDatabaseDetails, "{id}")
            .MapGet(GetDatabasesList, "List")
            .MapGet(GetTablesList, "{id}/Tables/List")
            .MapGet(GetColumnsList, "{id}/Tables/{tableId}/Columns/List")
            .MapGet(TestConnection, "TestConnection")
            .MapPost(CreateDatabase)
            .MapPost(PopulateDatabase, "{id}/Populate")
            .MapPost(ClearDatabase, "{id}/Clear")
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

    public async Task<Ok<List<IdNameDto>>> GetDatabasesList(ISender sender)
    {
        var query = new GetDatabasesListQuery()
        {

        };

        var result = await sender.Send(query);

        return TypedResults.Ok(result);
    }

    public async Task<Ok<List<IdNameDto>>> GetTablesList(ISender sender, int id)
    {
        var query = new GetTablesListQuery()
        {
            DatabaseId = id
        };

        var result = await sender.Send(query);

        return TypedResults.Ok(result);
    }

    public async Task<Ok<List<IdNameDto>>> GetColumnsList(ISender sender, int id, int tableId, bool onlyNumeric)
    {
        var query = new GetColumnsListQuery()
        {
            DatabaseId = id,
            DatabaseTableId = tableId,
            OnlyNumeric = onlyNumeric
        };

        var result = await sender.Send(query);

        return TypedResults.Ok(result);
    }

    public async Task<Ok<DatabaseDetailsDto>> GetDatabaseDetails(ISender sender, int id)
    {
        var query = new GetDatabaseDetailsQuery()
        {
            DatabaseId = id
        };

        var result = await sender.Send(query);

        return TypedResults.Ok(result);
    }

    public async Task<Ok<bool>> TestConnection(ISender sender, DatabaseProvider databaseProvider, string connectionString)
    {
        var query = new TestConnectionQuery()
        {
            DatabaseProvider = databaseProvider,
            ConnectionString = connectionString
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
    public async Task<Ok<int>> ClearDatabase(ISender sender, int id)
    {
        var command = new ClearDatabaseCommand() { Id = id };

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
