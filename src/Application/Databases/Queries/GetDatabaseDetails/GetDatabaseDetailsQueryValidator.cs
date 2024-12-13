using DataVision.Application.Common.Interfaces;
using DataVision.Application.Databases.Queries.GetDatabases;

namespace DataVision.Application.Databases.Queries.GetDatabaseDetails;
public class GetDatabaseDetailsQueryValidator : AbstractValidator<GetDatabaseDetailsQuery>
{
    public GetDatabaseDetailsQueryValidator(IExistenceService existenceService)
    {
        RuleFor(v => v.DatabaseId).MustAsync(existenceService.DatabaseExistsAsync);
    }
}
