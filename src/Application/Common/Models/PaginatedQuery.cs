namespace DataVision.Application.Common.Models;
public record PaginatedQuery
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
}
