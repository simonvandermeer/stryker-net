using Newtonsoft.Json;

namespace Stryker.Abstractions.Reporting;

public interface ILocation
{
    [JsonProperty("end")]
    IPosition End { get; init; }

    [JsonProperty("start")]
    IPosition Start { get; init; }
}
