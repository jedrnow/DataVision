using DataVision.Application.BackgroundJobs.Queries.GetBackgroundJobDetails;
using DataVision.Application.BackgroundJobs.Queries.GetBackgroundJobHistory;
using DataVision.Application.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DataVision.Web.Endpoints;

public class BackgroundJobs : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetBackgroundJobDetails, "{id}")
            .MapGet(GetBackgroundJobHistory, "History");
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

    public async Task<Ok<PaginatedList<BackgroundJobDetailsDto>>> GetBackgroundJobHistory(ISender sender, int databaseId, int pageNumber, int pageSize)
    {
        var query = new GetBackgroundJobHistoryQuery()
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
