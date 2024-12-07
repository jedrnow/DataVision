using DataVision.Application.Common.Interfaces;

namespace DataVision.Application.Common.Services;
public class ExistenceService : IExistenceService
{
    IApplicationDbContext _context { get; set; }
    public ExistenceService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> DatabaseExistsAsync(int databaseId, CancellationToken cancellationToken = default)
    {
        var database = await _context.Databases
            .AsNoTracking()
            .SingleOrDefaultAsync(cancellationToken);

        return database != null;
    }
}
