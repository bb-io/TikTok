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

public class VideoResultResponse
{
    [Display("Video URL")]
    [JsonProperty("video")]
    public string Url { get; set; } = string.Empty;

    [Display("Video URL expires at")]
    [JsonProperty("video_expire_time")]
    public DateTime? ExpiresAt { get; set; }

    [Display("Last frame URL")]
    [JsonProperty("last_frame_url")]
    public string? LastFrameUrl { get; set; }

    [Display("Duration in seconds")]
    [JsonProperty("duration")]
    public double? Duration { get; set; }

    [Display("Resolution")]
    [JsonProperty("resolution")]
    public string? Resolution { get; set; }

    [Display("Aspect ratio")]
    [JsonProperty("ratio")]
    public string? AspectRatio { get; set; }

    [Display("Frames per second")]
    [JsonProperty("frames_per_second")]
    public double? FramesPerSecond { get; set; }
}
