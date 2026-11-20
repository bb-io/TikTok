using Apps.TikTokSymphony.Handlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.TikTokSymphony.Models.Identifiers;

public class DownloadTaskIdentifier : TaskIdentifier
{
    [Display("Video task ID", Description = "Search succeeded tasks available to this connection, or enter/map a video task ID.")]
    [DataSource(typeof(SucceededTaskDataHandler))]
    public override string TaskId { get; set; } = string.Empty;
}
