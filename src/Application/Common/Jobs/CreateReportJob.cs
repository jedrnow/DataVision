using DataVision.Application.Common.Documents;
using DataVision.Application.Common.Interfaces;
using DataVision.Application.Common.Models;
using DataVision.Domain.Entities;
using DataVision.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DataVision.Application.Common.Jobs;
public class CreateReportJob
{
    private readonly IApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    public CreateReportJob(IApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task Run(int jobId, string? userId, string? title, int databaseId, ReportFormat? format, List<int> tableIds, CancellationToken cancellationToken = default)
    {
        var job = await _context.BackgroundJobs.SingleOrDefaultAsync(x => x.Id == jobId, cancellationToken);
        Guard.Against.NotFound(jobId, job);
        Guard.Against.Null(format, nameof(format));
        Guard.Against.NullOrEmpty(title, nameof(title));

        var result = new CreateReportResult()
        {
            Title = title,
            Format = format,
            TableIds = tableIds,
        };

        try
        {
            var tables = await _context.DatabaseTables
                .AsNoTracking()
                .Include(x => x.Columns)
                .Include(x => x.Rows)
                    .ThenInclude(r => r.Cells)
                .Where(x => x.DatabaseId == databaseId && tableIds.Contains(x.Id))
                .ToListAsync(cancellationToken);

            var report = new Report()
            {
                DatabaseId = databaseId,
                Title = title,
                Format = format.Value,
                CreatedBy = userId,
                LastModifiedBy = userId,
            };

            if (format == ReportFormat.Pdf)
            {
                var fileName = $"Database_{databaseId}_Report_{DateTime.UtcNow:yyyyMMdd_HHmmss}.pdf";

                var document = new DatabaseReportDocument(tables, fileName, _configuration);
                await document.GeneratePdfAndUploadAsync();

                report.FileName = fileName;
                result.FileName = fileName;
            }
            else if(format == ReportFormat.Xlsx)
            {
                var fileName = $"Database_{databaseId}_Report_{DateTime.UtcNow:yyyyMMdd_HHmmss}.xlsx";

                var document = new DatabaseReportDocument(tables, fileName, _configuration);
                await document.GenerateXlsxAndUploadAsync();

                report.FileName = fileName;
                result.FileName = fileName;
            }else if(format == ReportFormat.Html)
            {
                var fileName = $"Database_{databaseId}_Report_{DateTime.UtcNow:yyyyMMdd_HHmmss}.html";
                var document = new DatabaseReportDocument(tables, fileName, _configuration);
                await document.GenerateHtmlAndUploadAsync();
                report.FileName = fileName;
                result.FileName = fileName;
            }

            _context.Reports.Add(report);
            result.Success = true;
            job.IsSucceeded = true;
            job.IsCompleted = true;
            job.Result = JsonConvert.SerializeObject(result);
        }
        catch (Exception ex)
        {
            result.Message = ex.Message;
            job.IsSucceeded = false;
            job.IsCompleted = true;
            job.Result = JsonConvert.SerializeObject(result);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
