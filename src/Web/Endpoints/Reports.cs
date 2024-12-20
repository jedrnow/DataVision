using DataVision.Application.Reports.Commands.CreateReport;
using DataVision.Application.Reports.Commands.DeleteReport;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DataVision.Web.Endpoints;

public class Reports : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapPost(CreateReport)
            .MapDelete(DeleteReport, "{id}");
    }

    public async Task<Created<int>> CreateReport(ISender sender, CreateReportCommand command)
    {
        var id = await sender.Send(command);

        return TypedResults.Created($"/{nameof(Reports)}/{id}", id);
    }

    public async Task<Ok<bool>> DeleteReport(ISender sender, int id)
    {
        var command = new DeleteReportCommand() { Id = id };

        var result = await sender.Send(command);

        return TypedResults.Ok(result);
    }
}
