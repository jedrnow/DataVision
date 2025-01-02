using DataVision.Application.Common.Interfaces;
using DataVision.Application.Common.Validators;

namespace DataVision.Application.Reports.Commands.CreateReport;
public class CreateReportCommandValidator : AbstractValidator<CreateReportCommand>
{
    public CreateReportCommandValidator(IExistenceService existenceService)
    {
        RuleFor(v => v.Title)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(v => v.DatabaseId)
            .NotEmpty()
            .MustAsync(existenceService.DatabaseExistsAsync);

        RuleFor(v => v.Format)
            .NotNull()
            .NotEmpty()
            .IsInEnum();

        RuleForEach(v => v.Charts)
            .SetValidator(new ReportChartModelValidator(existenceService));
    }
}
