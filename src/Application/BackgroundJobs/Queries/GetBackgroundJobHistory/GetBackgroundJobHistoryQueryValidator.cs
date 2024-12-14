using DataVision.Application.Common.Interfaces;
using DataVision.Application.Common.Validators;

namespace DataVision.Application.BackgroundJobs.Queries.GetBackgroundJobHistory;
public class GetBackgroundJobHistoryQueryValidator : AbstractValidator<GetBackgroundJobHistoryQuery>
{
    public GetBackgroundJobHistoryQueryValidator(IExistenceService existenceService)
    {
        RuleFor(v => v.DatabaseId).MustAsync(existenceService.DatabaseExistsAsync);

        RuleFor(v => v.PaginatedQuery).SetValidator(new PaginationValidator());
    }
}
