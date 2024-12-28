using ClosedXML.Excel;
using DataVision.Domain.Entities;
using Google.Cloud.Storage.V1;
using HtmlAgilityPack;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace DataVision.Application.Common.Documents
{
    public class DatabaseReportDocument
    {
        private readonly List<DatabaseTable> _tables;
        private readonly string _fileName;
        private readonly IConfiguration _configuration;

        public DatabaseReportDocument(List<DatabaseTable> tables, string fileName, IConfiguration configuration)
        {
            _tables = tables;
            _fileName = fileName;
            _configuration = configuration;
        }

        public async Task GeneratePdfAndUploadAsync()
        {
            var memoryStream = new MemoryStream();

            using (var writer = new PdfWriter(memoryStream))
            using (var pdf = new PdfDocument(writer))
            {
                var document = new iText.Layout.Document(pdf);

                var counter = _tables.Count;
                foreach (var table in _tables)
                {
                    document.Add(new Paragraph(table.Name)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(16));

                    var pdfTable = new Table(table.Columns.Count, true).SetAutoLayout();

                    foreach (var column in table.Columns)
                    {
                        pdfTable.AddHeaderCell(new Cell().Add(new Paragraph(column.Name)));
                    }

                    foreach (var row in table.Rows)
                    {
                        foreach (var cell in row.Cells)
                        {
                            pdfTable.AddCell(new Cell().Add(new Paragraph(cell.Value)));
                        }
                    }

                    document.Add(pdfTable);
                    pdfTable.Complete();
                    counter--;
                    if (counter > 0)
                    {
                        document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                    }
                }

                document.Close();
            }

            var storageClient = StorageClient.Create();
            var bucketName = _configuration.GetSection("GoogleCloudStorage").GetValue<string>("ContainerName");

            var pdfStream = memoryStream.ToArray();

            using var stream = new MemoryStream(pdfStream);
            await storageClient.UploadObjectAsync(bucketName, _fileName, "application/pdf", stream);
        }

        public async Task GenerateXlsxAndUploadAsync()
        {
            var memoryStream = new MemoryStream();

            using (var workbook = new XLWorkbook())
            {
                foreach (var table in _tables)
                {
                    var worksheet = workbook.Worksheets.Add(table.Name ?? table.Id.ToString());

                    // Add column headers
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        worksheet.Cell(1, i + 1).Value = table.Columns[i].Name;
                    }

                    // Add rows and cells
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        var row = table.Rows[i];
                        for (int j = 0; j < row.Cells.Count; j++)
                        {
                            worksheet.Cell(i + 2, j + 1).Value = row.Cells[j].Value;
                        }
                    }

                    worksheet.Columns().AdjustToContents();
                }

                workbook.SaveAs(memoryStream);
            }

            var storageClient = StorageClient.Create();
            var bucketName = _configuration.GetSection("GoogleCloudStorage").GetValue<string>("ContainerName");

            var xlsxStream = memoryStream.ToArray();

            using var stream = new MemoryStream(xlsxStream);
            await storageClient.UploadObjectAsync(bucketName, _fileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", stream);
        }

        public async Task GenerateHtmlAndUploadAsync()
        {
            var htmlDocument = new HtmlDocument();
            var htmlBuilder = new StringBuilder();

            htmlBuilder.Append("<html><head><title>Database Report</title>");
            htmlBuilder.Append("<script src='https://cdn.jsdelivr.net/npm/chart.js'></script>");
            htmlBuilder.Append("</head><body>");

            foreach (var table in _tables)
            {
                htmlBuilder.Append($"<h2>{table.Name}</h2>");
                htmlBuilder.Append("<table border='1'><tr>");

                // Add column headers
                foreach (var column in table.Columns)
                {
                    htmlBuilder.Append($"<th>{column.Name}</th>");
                }
                htmlBuilder.Append("</tr>");

                // Add rows and cells
                foreach (var row in table.Rows)
                {
                    htmlBuilder.Append("<tr>");
                    foreach (var cell in row.Cells)
                    {
                        htmlBuilder.Append($"<td>{cell.Value}</td>");
                    }
                    htmlBuilder.Append("</tr>");
                }
                htmlBuilder.Append("</table>");

                // Add chart
                var chartId = $"chart_{table.Id}";
                htmlBuilder.Append($"<canvas id='{chartId}'></canvas>");
                htmlBuilder.Append("<script>");
                htmlBuilder.Append($"var ctx = document.getElementById('{chartId}').getContext('2d');");
                htmlBuilder.Append("var chart = new Chart(ctx, { type: 'bar', data: { labels: [");

                // Add chart labels
                foreach (var column in table.Columns)
                {
                    htmlBuilder.Append($"'{column.Name}',");
                }
                htmlBuilder.Append("], datasets: [{ label: 'Data', data: [");

                // Add chart data
                foreach (var row in table.Rows)
                {
                    foreach (var cell in row.Cells)
                    {
                        if (decimal.TryParse(cell.Value, out var value))
                        {
                            htmlBuilder.Append($"{value},");
                        }
                        else
                        {
                            htmlBuilder.Append("0,");
                        }
                    }
                }
                htmlBuilder.Append("] }] } });");
                htmlBuilder.Append("</script>");
            }

            htmlBuilder.Append("</body></html>");

            htmlDocument.LoadHtml(htmlBuilder.ToString());

            var memoryStream = new MemoryStream();
            var writer = new StreamWriter(memoryStream);
            writer.Write(htmlDocument.DocumentNode.OuterHtml);
            writer.Flush();
            memoryStream.Position = 0;

            var storageClient = StorageClient.Create();
            var bucketName = _configuration.GetSection("GoogleCloudStorage").GetValue<string>("ContainerName");

            await storageClient.UploadObjectAsync(bucketName, _fileName, "text/html", memoryStream);
        }
    }
}
