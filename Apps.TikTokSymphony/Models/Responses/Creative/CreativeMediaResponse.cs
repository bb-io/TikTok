using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.TikTokSymphony.Models.Responses.Creative;

public class CreativeMediaResponse
{
    [Display("Creative video ID")]
    [JsonProperty("video_id")]
    public virtual string? VideoId { get; set; }

    [Display("Name")]
    [JsonProperty("video_name")]
    public string? Name { get; set; }

    [Display("Video URL")]
    [JsonProperty("preview_url")]
    public string? PreviewUrl { get; set; }

    [Display("Subtitle URL")]
    [JsonProperty("srt_file_url")]
    public string? SubtitleUrl { get; set; }
}
