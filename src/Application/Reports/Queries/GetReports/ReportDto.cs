using DataVision.Domain.Entities;
using DataVision.Domain.Enums;

namespace DataVision.Application.Reports.Queries.GetReports;
public class ReportDto
{
    public int Id { get; set; }
    public int DatabaseId { get; set; }
    public string? Title { get; set; }
    public ReportFormat Format { get; set; }
    public string? FileName { get; set; }
    public DateTimeOffset Created { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset LastModified { get; set; }
    public string? LastModifiedBy { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Report, ReportDto>();
        }
    }
}
