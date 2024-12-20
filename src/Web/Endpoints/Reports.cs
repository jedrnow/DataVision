using DataVision.Application.Reports.Commands.CreateReport;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DataVision.Web.Endpoints;

public class Reports : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapPost(CreateReport);
    }

    public async Task<Created<int>> CreateReport(ISender sender, CreateReportCommand command)
    {
        var id = await sender.Send(command);

        return TypedResults.Created($"/{nameof(Reports)}/{id}", id);
    }
}
