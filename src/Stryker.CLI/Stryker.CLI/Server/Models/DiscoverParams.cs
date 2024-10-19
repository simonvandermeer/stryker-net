using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Stryker.CLI.Server.Models;

public class DiscoverParams
{
    public const string CommandName = "discover";

    [JsonPropertyName("globPatterns")]
    public IEnumerable<string> GlobPatterns { get; set; }
}
