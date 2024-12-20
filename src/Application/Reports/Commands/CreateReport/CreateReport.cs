using Azure.Storage.Blobs;
using DataVision.Application.Common.Interfaces;
using DataVision.Domain.Entities;
using DataVision.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace DataVision.Application.Reports.Commands.CreateReport;

public record CreateReportCommand : IRequest<int>
{
    public int DatabaseId { get; init; }
    public string? Title { get; init; }
    public IFormFile? File { get; init; }
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
        Guard.Against.Null(request.File, nameof(request.File));

        var fileName = request.DatabaseId + "_" + request.File.FileName;

        var report = new Report()
        {
            DatabaseId = request.DatabaseId,
            Title = request.Title,
            Format = GetFormat(fileName),
            FileName = fileName,
        };

        var containerClient = _blobServiceClient.GetBlobContainerClient(BlobContainerNames.reports.ToString());
        var blobClient = containerClient.GetBlobClient(fileName);

        await using var stream = request.File.OpenReadStream();
        await blobClient.UploadAsync(stream, overwrite: true);

        _context.Reports.Add(report);

        await _context.SaveChangesAsync(cancellationToken);

        return report.Id;
    }

    private static ReportFormat GetFormat(string fileName)
    {
        if (fileName.EndsWith(".pdf"))
        {
            return ReportFormat.Pdf;
        }

        if (fileName.EndsWith(".xlsx"))
        {
            return ReportFormat.Xlsx;
        }

        throw new NotImplementedException();
    }
}
