using DataVision.Application.Common.Interfaces;

namespace DataVision.Application.Databases.Queries.GetColumnsList;
public class GetColumnsListQueryValidator : AbstractValidator<GetColumnsListQuery>
{
    public GetColumnsListQueryValidator(IExistenceService existenceService)
    {
        RuleFor(x => x.DatabaseId).MustAsync(existenceService.DatabaseExistsAsync);

        RuleFor(x => x.DatabaseTableId).MustAsync(existenceService.DatabaseTableExistsAsync);
    }
}
