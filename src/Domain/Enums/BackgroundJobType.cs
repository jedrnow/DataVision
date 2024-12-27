using System.Text.Json.Serialization;

namespace DataVision.Domain.Enums;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum BackgroundJobType
{
    PopulateDatabase,
    ClearDatabase,
    DeleteDatabase,
    CreateReport,
}
