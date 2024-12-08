using DataVision.Application.Common.Interfaces;

namespace DataVision.Application.DatabaseTables.Queries.GetDatabaseTables;
public class GetDatabaseTableColumnsQueryValidator : AbstractValidator<GetDatabaseTableColumnsQuery>
{
    public GetDatabaseTableColumnsQueryValidator(IExistenceService existenceService)
    {
        RuleFor(x => x.DatabaseTableId).MustAsync(existenceService.DatabaseTableExistsAsync);
    }
}
