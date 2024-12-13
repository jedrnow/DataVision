using DataVision.Application.BackgroundJobs.Queries.GetBackgroundJobDetails;
using DataVision.Application.Common.Interfaces;

namespace DataVision.Application.Databases.Queries.GetDatabaseDetails;
public class GetBackgroundJobDetailsQueryValidator : AbstractValidator<GetBackgroundJobDetailsQuery>
{
    public GetBackgroundJobDetailsQueryValidator(IExistenceService existenceService)
    {
        RuleFor(v => v.BackgroundJobId).MustAsync(existenceService.BackgroundJobExistsAsync);
    }
}
