using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.TikTokSymphony.Models.Responses.Creative;

public class AvatarTaskResponse : CreativeTaskResponse
{
    [Display("Avatar video ID")]
    [JsonProperty("avatar_video_id")]
    public override string? VideoId { get; set; }
}
