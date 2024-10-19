using Newtonsoft.Json;

namespace Stryker.Abstractions.Reporting;

public interface IPosition
{
    [JsonProperty("column")]
    int Column { get; set; }

    [JsonProperty("line")]
    int Line { get; set; }
}
