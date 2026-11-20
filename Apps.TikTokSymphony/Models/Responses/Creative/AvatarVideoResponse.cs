using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.TikTokSymphony.Models.Responses.Creative;

public class AvatarVideoResponse
{
    [Display("Avatar video ID")]
    [JsonProperty("avatar_video_id")]
    public string? VideoId { get; set; }

    [Display("Name")]
    [JsonProperty("video_name")]
    public string? Name { get; set; }

    [Display("Created at")]
    [JsonProperty("create_time")]
    public DateTime? CreatedAt { get; set; }

    [Display("Video URL")]
    [JsonProperty("preview_url")]
    public string? PreviewUrl { get; set; }

    [Display("Avatar ID")]
    [JsonProperty("avatar_id")]
    public string? AvatarId { get; set; }

    [Display("Cover URL")]
    [JsonProperty("video_cover_url")]
    public string? CoverUrl { get; set; }
}
