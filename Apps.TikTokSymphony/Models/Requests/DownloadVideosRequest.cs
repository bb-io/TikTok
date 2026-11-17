using Apps.TikTokSymphony.Handlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.TikTokSymphony.Models.Requests;

public class DownloadVideosRequest
{
    [Display("Video task IDs", Description = "Select, enter, or map up to 100 succeeded task IDs accessible through this connection. Every requested task must be available and succeeded.")]
    [DataSource(typeof(SucceededTaskDataHandler))]
    public List<string> TaskIds { get; set; } = [];
}
