using Newtonsoft.Json;

namespace Stryker.LanguageServer.Models;

public class ConfigureParams
{
    public const string CommandName = "configure";

    [JsonProperty("configFilePath")]
    public string? ConfigFilePath { get; set; }
}
