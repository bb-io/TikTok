using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.TikTokSymphony.Models.Responses.Creative;

public class CreativeVideoResponse : CreativeMediaResponse
{
    [Display("Created at")]
    [JsonProperty("create_time")]
    public DateTime? CreatedAt { get; set; }

    [Display("Dubbing information")]
    [JsonProperty("dubbing_info")]
    public DubbingInfoResponse? DubbingInfo { get; set; }
}
