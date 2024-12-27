using DataVision.Application.Common.Interfaces;

namespace DataVision.Application.Reports.Commands.DownloadReport;
public class DownloadReportCommandValidator : AbstractValidator<DownloadReportCommand>
{
    public DownloadReportCommandValidator(IExistenceService existenceService)
    {
        RuleFor(v => v.ReportId).MustAsync(existenceService.ReportExistsAsync);
    }
}

