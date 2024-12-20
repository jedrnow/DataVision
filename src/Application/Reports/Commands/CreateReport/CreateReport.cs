using Azure.Storage.Blobs;
using DataVision.Application.Common.Documents;
using DataVision.Application.Common.Interfaces;
using DataVision.Domain.Entities;
using DataVision.Domain.Enums;
using QuestPDF.Fluent;

namespace DataVision.Application.Reports.Commands.CreateReport;

public record CreateReportCommand : IRequest<int>
{
    public int DatabaseId { get; init; }
    public string? Title { get; init; }
    public List<int> TableIds { get; init; } = [];
}

public class CreateReportCommandHandler : IRequestHandler<CreateReportCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly BlobServiceClient _blobServiceClient;

    public CreateReportCommandHandler(IApplicationDbContext context, BlobServiceClient blobServiceClient)
    {
        _context = context;
        _blobServiceClient = blobServiceClient;
    }

    public async Task<int> Handle(CreateReportCommand request, CancellationToken cancellationToken)
    {
        var tables = await _context.DatabaseTables
            .AsNoTracking()
            .Include(x => x.Columns)
            .Include(x => x.Rows)
                .ThenInclude(r => r.Cells)
            .Where(x => x.DatabaseId == request.DatabaseId && request.TableIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

        var fileName = $"Database_{request.DatabaseId}_Report_{DateTime.UtcNow:yyyyMMdd_HHmmss}.pdf";

        using var memoryStream = new MemoryStream();
        var document = new DatabaseReportDocument(tables);
        document.GeneratePdf(memoryStream);

        memoryStream.Position = 0;

        var containerClient = _blobServiceClient.GetBlobContainerClient(BlobContainerNames.reports.ToString());
        var blobClient = containerClient.GetBlobClient(fileName);

        await blobClient.UploadAsync(memoryStream, overwrite: true, cancellationToken: cancellationToken);

        var report = new Report()
        {
            DatabaseId = request.DatabaseId,
            Title = request.Title,
            Format = ReportFormat.Pdf,
            FileName = fileName,
        };

        _context.Reports.Add(report);

        await _context.SaveChangesAsync(cancellationToken);

        return report.Id;
    }
}
