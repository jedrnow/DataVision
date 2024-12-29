using DataVision.Application.Common.Interfaces;
using DataVision.Application.Common.Models;

namespace DataVision.Application.Common.Validators;
public class ReportChartModelValidator : AbstractValidator<ReportChartModel>
{
    public ReportChartModelValidator(IExistenceService existenceService)
    {
        RuleFor(x => x.Title).NotEmpty();

        RuleFor(x => x.ChartType).IsInEnum();

        RuleFor(x => x.TargetColumnId).MustAsync(existenceService.DatabaseTableColumnExistsAsync);

        RuleFor(x => x.LabelColumnId).MustAsync(existenceService.DatabaseTableColumnExistsAsync);

        RuleFor(x => x.TableId).MustAsync(existenceService.DatabaseTableExistsAsync);
    }
}
