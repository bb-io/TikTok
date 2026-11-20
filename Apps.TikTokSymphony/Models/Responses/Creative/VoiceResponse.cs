using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.TikTokSymphony.Models.Responses.Creative;

public class VoiceResponse
{
    [Display("Voice ID")]
    [JsonProperty("voice_id")]
    public string VoiceId { get; set; } = string.Empty;

    [Display("Name")]
    [JsonProperty("voice_name")]
    public string Name { get; set; } = string.Empty;

    [Display("Preview URL")]
    [JsonProperty("preview_url")]
    public string? PreviewUrl { get; set; }

    [Display("Tags")]
    [JsonProperty("voice_tags")]
    public List<VoiceTagResponse> Tags { get; set; } = [];
}
