using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.TikTokSymphony.Models.Responses.Creative;

public class AvatarTagResponse
{
    [Display("Tag type")]
    [JsonProperty("tag_type")]
    public string Type { get; set; } = string.Empty;

    [Display("Tags")]
    [JsonProperty("tags")]
    public List<string> Tags { get; set; } = [];
}
