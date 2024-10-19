using System.Collections.Generic;
using Newtonsoft.Json;

namespace Stryker.CLI.Server.Models;

public class DiscoverResult
{
    [JsonProperty("mutants")]
    public IEnumerable<DiscoveredMutant> Mutants { get; set; }
}
