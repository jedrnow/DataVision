namespace DataVision.Application.Databases.Queries.TestConnection;
public class TestConnectionQueryValidator : AbstractValidator<TestConnectionQuery>
{
    public TestConnectionQueryValidator()
    {
        RuleFor(v => v.ConnectionString)
            .NotEmpty();

        RuleFor(v => v.DatabaseProvider)
            .IsInEnum();
    }
}
