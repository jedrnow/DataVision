using DataVision.Application.Common.Interfaces;

namespace DataVision.Application.Reports.Commands.UpdateReport;
public class UpdateReportCommandValidator : AbstractValidator<UpdateReportCommand>
{
    public UpdateReportCommandValidator(IExistenceService existenceService)
    {
        RuleFor(v => v.Id).MustAsync(existenceService.ReportExistsAsync);

        RuleFor(v => v.Title)
            .NotEmpty()
            .MaximumLength(255);
    }
}
