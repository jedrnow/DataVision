using DataVision.Application.Common.Factories;
using DataVision.Domain.Enums;

namespace DataVision.Application.Databases.Queries.TestConnection;

public record TestConnectionQuery : IRequest<bool>
{
    public DatabaseProvider DatabaseProvider { get; set; }
    public required string ConnectionString { get; set; }
}

public class TestConnectionQueryHandler : IRequestHandler<TestConnectionQuery, bool>
{

    public TestConnectionQueryHandler()
    {

    }

    public async Task<bool> Handle(TestConnectionQuery request, CancellationToken cancellationToken)
    {
        var adapter = DatabaseAdapterFactory.CreateAdapter(request.DatabaseProvider, request.ConnectionString);

        var result = await adapter.CanConnectAsync();

        return result;
    }
}
