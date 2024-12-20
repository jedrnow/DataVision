using DataVision.Application.Databases.Commands.DeleteDatabase;
using DataVision.Application.Databases.Commands.UpdateDatabase;
using DataVision.Application.Reports.Commands.CreateReport;
using DataVision.Application.Reports.Commands.DeleteReport;
using DataVision.Application.Reports.Commands.UpdateReport;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DataVision.Web.Endpoints;

public class Reports : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapPost(CreateReport)
            .MapPut(UpdateReport, "{id}")
            .MapDelete(DeleteReport, "{id}");
    }

    public async Task<Created<int>> CreateReport(ISender sender, CreateReportCommand command)
    {
        var id = await sender.Send(command);

        return TypedResults.Created($"/{nameof(Reports)}/{id}", id);
    }

    public async Task<Results<Ok<bool>, BadRequest>> UpdateReport(ISender sender, int id, UpdateReportCommand command)
    {
        if (id != command.Id) return TypedResults.BadRequest();

        var result = await sender.Send(command);

        return TypedResults.Ok(result);
    }

    public async Task<Ok<bool>> DeleteReport(ISender sender, int id)
    {
        var command = new DeleteReportCommand() { Id = id };

        var result = await sender.Send(command);

        return TypedResults.Ok(result);
    }
}
