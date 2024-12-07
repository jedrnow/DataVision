using DataVision.Application.Common.Interfaces;

namespace DataVision.Application.Databases.Commands.DeleteDatabase;
public class DeleteDatabaseCommandValidator : AbstractValidator<DeleteDatabaseCommand>
{
    public DeleteDatabaseCommandValidator(IExistenceService existenceService)
    {
        RuleFor(v => v.Id).MustAsync(existenceService.DatabaseExistsAsync);
    }
}

