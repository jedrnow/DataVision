namespace DataVision.Application.Common.Models;
public record DatabaseMappingResult
{
    public bool Success { get; set; } = true;
    public string DatabaseName { get; set; } = string.Empty;
    public int TablesTotal { get; set; } = 0;
    public List<string> TableNames { get; set; } = [];
    public List<string> Errors { get; set; } = [];
}
