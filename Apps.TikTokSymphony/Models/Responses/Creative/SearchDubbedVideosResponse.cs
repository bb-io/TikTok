using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.TikTokSymphony.Models.Responses.Creative;

public class SearchDubbedVideosResponse
{
    [Display("Videos")]
    public List<CreativeVideoResponse> Videos { get; set; } = [];
}
