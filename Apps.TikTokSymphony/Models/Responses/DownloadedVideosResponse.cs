using Blackbird.Applications.Sdk.Common;

namespace Apps.TikTokSymphony.Models.Responses;

public class DownloadedVideosResponse
{
    [Display("Videos")]
    public List<DownloadedVideoResponse> Videos { get; set; } = [];
}
