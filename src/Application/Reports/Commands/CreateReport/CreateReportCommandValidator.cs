using DataVision.Application.Common.Interfaces;

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

        RuleFor(v => v.TableIds)
            .NotEmpty();
    }
}
