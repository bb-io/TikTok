using Blackbird.Applications.Sdk.Common;

namespace Apps.TikTokSymphony.Models.Requests;

public class ExtendVideoRequest : VideoTransformRequest
{
    [Display("Duration in seconds", Description = "4–30, or -1 with paid tokens to let AI decide. Leave empty for TikTok's default. TikTok allows extending a video at most twice.")]
    public int? Duration { get; set; }
}
