using Apps.TikTokSymphony.Handlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.TikTokSymphony.Models.Identifiers;

public class LastFrameTaskIdentifier : TaskIdentifier
{
    [Display("Video task ID", Description = "Search succeeded tasks with a last frame, or enter/map a video task ID. Last-frame generation requires paid-token access.")]
    [DataSource(typeof(LastFrameTaskDataHandler))]
    public override string TaskId { get; set; } = string.Empty;
}
