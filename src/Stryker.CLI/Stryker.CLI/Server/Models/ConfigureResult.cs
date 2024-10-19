using Newtonsoft.Json;

namespace Stryker.LanguageServer.Models;

public class ConfigureResult
{
    [JsonProperty("version")]
    public string Version { get; set; }
}
