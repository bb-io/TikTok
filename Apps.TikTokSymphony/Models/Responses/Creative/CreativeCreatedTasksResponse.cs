using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.TikTokSymphony.Models.Responses.Creative;

public class CreativeCreatedTasksResponse
{
    [Display("Creative task IDs")]
    [JsonProperty("task_ids")]
    public List<string> TaskIds { get; set; } = [];
}
