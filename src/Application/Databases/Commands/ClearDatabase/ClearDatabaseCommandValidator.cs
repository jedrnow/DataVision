using DataVision.Application.Common.Interfaces;

namespace DataVision.Application.Databases.Commands.ClearDatabase;
public class ClearDatabaseCommandValidator : AbstractValidator<ClearDatabaseCommand>
{
    public ClearDatabaseCommandValidator(IExistenceService existenceService)
    {
        RuleFor(v => v.Id).MustAsync(existenceService.DatabaseExistsAsync);
    }
}

