using System.Text.Json.Serialization;

namespace DataVision.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ChartType
{
    Bar,
    Line,
    Pie,
}
