using Apps.TikTokSymphony.Handlers.Static;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.TikTokSymphony.Models.Requests;

public class GenerationUsageRequest
{
    [Display("Created after", Description = "UTC task creation time. Only tasks created within the last seven days are available.")]
    public DateTime? CreatedAfter { get; set; }

    [Display("Created before", Description = "UTC task creation time. This is a task-creation window, not a billing transaction date range.")]
    public DateTime? CreatedBefore { get; set; }

    [Display("Task type")]
    [StaticDataSource(typeof(TaskTypeDataHandler))]
    public string? TaskType { get; set; }
}
