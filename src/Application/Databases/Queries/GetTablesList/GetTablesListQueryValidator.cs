using DataVision.Application.Common.Interfaces;

namespace DataVision.Application.Databases.Queries.GetTablesList;
public class GetTablesListQueryValidator : AbstractValidator<GetTablesListQuery>
{
    public GetTablesListQueryValidator(IExistenceService existenceService)
    {
        RuleFor(x => x.DatabaseId).MustAsync(existenceService.DatabaseExistsAsync);
    }
}
