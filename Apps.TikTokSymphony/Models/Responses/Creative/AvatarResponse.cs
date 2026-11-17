using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.TikTokSymphony.Models.Responses.Creative;

public class AvatarResponse
{
    [Display("Avatar ID")]
    [JsonProperty("avatar_id")]
    public string AvatarId { get; set; } = string.Empty;

    [Display("Name")]
    [JsonProperty("avatar_name")]
    public string Name { get; set; } = string.Empty;

    [Display("Thumbnail URL")]
    [JsonProperty("avatar_thumbnail")]
    public string? ThumbnailUrl { get; set; }

    [Display("Preview URL")]
    [JsonProperty("avatar_preview_url")]
    public string? PreviewUrl { get; set; }

    [Display("Tags")]
    [JsonProperty("tag_groups")]
    public List<AvatarTagResponse> Tags { get; set; } = [];
}
