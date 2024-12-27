using DataVision.Domain.Entities;
using Google.Cloud.Storage.V1;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.Extensions.Configuration;

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
                var document = new Document(pdf);

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
                    if(counter > 0)
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
    }
}
