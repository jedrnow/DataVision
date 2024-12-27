using DataVision.Application.Common.Models;
using DataVision.Application.Reports.Commands.CreateReport;
using DataVision.Application.Reports.Commands.DeleteReport;
using DataVision.Application.Reports.Queries.GetReports;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DataVision.Web.Endpoints;

public class Reports : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapPost(CreateReport)
            .MapGet(GetReports)
            .MapDelete(DeleteReport, "{id}");
    }

    public async Task<Ok<PaginatedList<ReportDto>>> GetReports(ISender sender, int pageNumber, int pageSize)
    {
        var query = new GetReportsQuery()
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
