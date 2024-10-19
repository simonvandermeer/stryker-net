using Newtonsoft.Json;
using Stryker.Abstractions.Reporting;

namespace Stryker.CLI.Server.Models;

public class DiscoveredMutant
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("location")]
    public ILocation Location { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("mutatorName")]
    public string MutatorName { get; set; }

    [JsonProperty("replacement")]
    public string Replacement { get; set; }
}
