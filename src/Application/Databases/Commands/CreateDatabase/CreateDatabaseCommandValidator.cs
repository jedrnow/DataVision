namespace DataVision.Application.Databases.Commands.CreateDatabase;
public class CreateDatabaseCommandValidator : AbstractValidator<CreateDatabaseCommand>
{
    public CreateDatabaseCommandValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(v => v.ConnectionString)
            .NotEmpty()
            .MustAsync(BeValidConnection)
                .WithMessage("'{PropertyName}' must provide valid connection")
                .WithErrorCode("400");

        RuleFor(v => v.DatabaseProvider)
            .IsInEnum();
    }

    public async Task<bool> BeValidConnection(string? connectionString, CancellationToken cancellationToken)
    {
        // TO DO
        await Task.Delay(1);

        return true;
    }
}
