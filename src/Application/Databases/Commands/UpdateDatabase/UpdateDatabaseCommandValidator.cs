namespace DataVision.Application.Databases.Commands.UpdateDatabase;
public class UpdateDatabaseCommandValidator : AbstractValidator<UpdateDatabaseCommand>
{
    public UpdateDatabaseCommandValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(v => v.ConnectionString)
            .NotEmpty()
            .MustAsync(BeValidConnection)
                .WithMessage("'{PropertyName}' must provide valid connection")
                .WithErrorCode("400");
    }

    public async Task<bool> BeValidConnection(string? connectionString, CancellationToken cancellationToken)
    {
        // TO DO
        await Task.Delay(1);

        return true;
    }
}
