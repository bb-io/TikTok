using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Apps.TikTokSymphony.Handlers;
using Apps.TikTokSymphony.Handlers.Static;
using Apps.TikTokSymphony.Models.Responses;
using Apps.TikTokSymphony.Models.Identifiers;

namespace Apps.TikTokSymphony.Models.Responses;

public class TaskSettingsResponse
{
    [Display("Output format")]
    [JsonProperty("output_format")]
    public string? OutputFormat { get; set; }

    [Display("Language")]
    [JsonProperty("language")]
    public string? Language { get; set; }
}
