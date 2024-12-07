using DataVision.Application.Common.Interfaces;
using DataVision.Application.Common.Validators;

namespace DataVision.Application.DatabaseTables.Queries.GetDatabaseTables;
public class GetDatabaseTablesQueryValidator : AbstractValidator<GetDatabaseTablesQuery>
{
    public GetDatabaseTablesQueryValidator(IExistenceService existenceService)
    {
        RuleFor(x => x.DatabaseId).MustAsync(existenceService.DatabaseExistsAsync);

        RuleFor(x => x.PaginatedQuery).SetValidator(new PaginationValidator());
    }
}
