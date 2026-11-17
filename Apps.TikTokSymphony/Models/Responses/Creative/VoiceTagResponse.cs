using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.TikTokSymphony.Models.Responses.Creative;

public class VoiceTagResponse
{
    [Display("Tag type")]
    [JsonProperty("tag_type")]
    public string Type { get; set; } = string.Empty;

    [Display("Tag name")]
    [JsonProperty("tag_name")]
    public string Name { get; set; } = string.Empty;
}
