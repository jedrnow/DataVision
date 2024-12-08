using DataVision.Application.Common.Interfaces;
using DataVision.Application.Common.Validators;

namespace DataVision.Application.DatabaseTables.Queries.GetDatabaseTables;
public class GetDatabaseTableRowsQueryValidator : AbstractValidator<GetDatabaseTableRowsQuery>
{
    public GetDatabaseTableRowsQueryValidator(IExistenceService existenceService)
    {
        RuleFor(x => x.DatabaseTableId).MustAsync(existenceService.DatabaseTableExistsAsync);

        RuleFor(x => x.PaginatedQuery).SetValidator(new PaginationValidator());
    }
}
