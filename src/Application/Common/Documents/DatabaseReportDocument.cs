using DataVision.Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace DataVision.Application.Common.Documents;

public class DatabaseReportDocument : IDocument
{
    private readonly List<DatabaseTable> _tables;

    public DatabaseReportDocument(List<DatabaseTable> tables)
    {
        _tables = tables;
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4);
            page.Margin(20);
            page.PageColor(Colors.White);
            page.DefaultTextStyle(x => x.FontSize(12));

            page.Content().Column(stack =>
            {
                foreach (var table in _tables)
                {
                    stack.Item().PaddingBottom(20).Table(tableBuilder =>
                    {
                        // Header Row
                        tableBuilder.ColumnsDefinition(columns =>
                        {
                            foreach (var _ in table.Columns)
                                columns.RelativeColumn();
                        });

                        tableBuilder.Header(header =>
                        {
                            foreach (var column in table.Columns)
                            {
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text(column.Name);
                            }
                        });

                        // Data Rows
                        foreach (var row in table.Rows)
                        {
                            tableBuilder.Cell().Row(rowBuilder =>
                            {
                                foreach (var cell in row.Cells)
                                {
                                    rowBuilder.AutoItem().Text(cell.Value);
                                }
                            });
                        }
                    });
                }
            });

            page.Footer().AlignCenter().Text("Generated Report - DataVision");
        });
    }
}
