﻿using DataVision.Application.Common.Documents;
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

    public async Task Run(int jobId, string? userId, CreateReportArgs args, CancellationToken cancellationToken = default)
    {
        var job = await _context.BackgroundJobs.SingleOrDefaultAsync(x => x.Id == jobId, cancellationToken);
        Guard.Against.NotFound(jobId, job);
        Guard.Against.Null(args.Format, nameof(args.Format));
        Guard.Against.NullOrEmpty(args.Title, nameof(args.Title));

        var tableIds = args.Tables.Select(t => t.TableId).ToList();

        var result = new CreateReportResult()
        {
            Title = args.Title,
            Format = args.Format,
            TableIds = tableIds,
        };

        try
        {
            var tables = await _context.DatabaseTables
                .AsNoTracking()
                .Include(x => x.Columns)
                .Include(x => x.Rows)
                    .ThenInclude(r => r.Cells)
                .Where(x => x.DatabaseId == args.DatabaseId && tableIds.Contains(x.Id))
                .ToListAsync(cancellationToken);

            var report = new Report()
            {
                DatabaseId = args.DatabaseId,
                Title = args.Title,
                Format = args.Format.Value,
                CreatedBy = userId,
                LastModifiedBy = userId,
            };

            var fileName = $"Database_{args.DatabaseId}_Report_{DateTime.UtcNow:yyyyMMdd_HHmmss}";
            if (args.Format == ReportFormat.Pdf)
            {
                fileName += ".pdf";

                var document = new DatabaseReportDocument(tables, args.Charts, args.Tables, fileName, args.GenerateTables, _configuration);
                await document.GeneratePdfAndUploadAsync();
            }
            else if (args.Format == ReportFormat.Xlsx)
            {
                fileName += ".xlsx";

                var document = new DatabaseReportDocument(tables, args.Charts, args.Tables, fileName, args.GenerateTables, _configuration);
                await document.GenerateXlsxAndUploadAsync();
            }
            else if (args.Format == ReportFormat.Html)
            {
                fileName += ".html";
                var document = new DatabaseReportDocument(tables, args.Charts, args.Tables, fileName, args.GenerateTables, _configuration);
                await document.GenerateHtmlAndUploadAsync();
            }
            else
            {
                throw new NotSupportedException($"Report format {args.Format} is not supported.");
            }

            report.FileName = fileName;
            result.FileName = fileName;

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
