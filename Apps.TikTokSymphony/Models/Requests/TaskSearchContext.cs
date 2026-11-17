using Apps.TikTokSymphony.Handlers.Static;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.TikTokSymphony.Models.Requests;

public class TaskSearchContext
{
    [Display("Status")]
    [StaticDataSource(typeof(TaskStatusDataHandler))]
    public string? Status { get; set; }

    [Display("Task type")]
    [StaticDataSource(typeof(TaskTypeDataHandler))]
    public string? TaskType { get; set; }

    [Display("Created after", Description = "UTC time.")]
    public DateTime? CreatedAfter { get; set; }

    [Display("Created before", Description = "UTC time.")]
    public DateTime? CreatedBefore { get; set; }
}
