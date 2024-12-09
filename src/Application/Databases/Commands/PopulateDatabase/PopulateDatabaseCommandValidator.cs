using DataVision.Application.Common.Interfaces;

namespace DataVision.Application.Databases.Commands.PopulateDatabase;
public class PopulateDatabaseCommandValidator : AbstractValidator<PopulateDatabaseCommand>
{
    public PopulateDatabaseCommandValidator(IExistenceService existenceService)
    {
        RuleFor(v => v.DatabaseId).MustAsync(existenceService.DatabaseExistsAsync);
    }
}
