using DataVision.Application.Common.Validators;

namespace DataVision.Application.Reports.Queries.GetReports;
public class GetReportsQueryValidator : AbstractValidator<GetReportsQuery>
{
    public GetReportsQueryValidator()
    {
        RuleFor(x => x.PaginatedQuery).SetValidator(new PaginationValidator());
    }
}
