using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.TikTokSymphony.Models.Responses.Creative;

public class SearchAvatarVideosResponse
{
    [Display("Videos")]
    public List<AvatarVideoResponse> Videos { get; set; } = [];
}
