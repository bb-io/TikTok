using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.TikTokSymphony.Models.Responses;

public class DownloadedVideoResponse
{
    [Display("Video task ID")]
    public string TaskId { get; set; } = string.Empty;

    [Display("File")]
    public FileReference File { get; set; } = default!;
}
