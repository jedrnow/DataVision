using DataVision.Application.Common.Interfaces;

namespace DataVision.Application.Reports.Commands.DeleteReport;
public class DeleteReportCommandValidator : AbstractValidator<DeleteReportCommand>
{
    public DeleteReportCommandValidator(IExistenceService existenceService)
    {
        RuleFor(v => v.Id).MustAsync(existenceService.ReportExistsAsync);
    }
}

