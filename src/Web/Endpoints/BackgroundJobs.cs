using DataVision.Application.BackgroundJobs.Queries.GetBackgroundJobDetails;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DataVision.Web.Endpoints;

public class BackgroundJobs : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetBackgroundJobDetails);
    }

    public async Task<Ok<BackgroundJobDetailsDto>> GetBackgroundJobDetails(ISender sender, int id)
    {
        var query = new GetBackgroundJobDetailsQuery()
        {
            BackgroundJobId = id
        };

        var result = await sender.Send(query);

        return TypedResults.Ok(result);
    }
}
