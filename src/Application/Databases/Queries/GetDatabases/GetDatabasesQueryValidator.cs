using DataVision.Application.Common.Validators;

namespace DataVision.Application.Databases.Queries.GetDatabases;
public class GetDatabasesQueryValidator : AbstractValidator<GetDatabasesQuery>
{
    public GetDatabasesQueryValidator()
    {
        RuleFor(x => x.PaginatedQuery).SetValidator(new PaginationValidator());
    }
}
