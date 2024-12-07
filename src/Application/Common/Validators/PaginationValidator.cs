using DataVision.Application.Common.Models;
using DataVision.Domain.Constants;

namespace DataVision.Application.Common.Validators;
public class PaginationValidator : AbstractValidator<PaginatedQuery>
{
    public PaginationValidator()
    {
        RuleFor(x => x.PageNumber)
            .InclusiveBetween(DefaultValues.MinPageNumber, DefaultValues.MaxPageNumber);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(DefaultValues.MinPageSize, DefaultValues.MaxPageSize);
    }
}
