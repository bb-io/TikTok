using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.TikTokSymphony.Models.Responses.Creative;

public class AvatarCreatedTaskResponse
{
    [Display("Avatar task ID")]
    [JsonProperty("task_id")]
    public string? TaskId { get; set; }

    [Display("Error message")]
    [JsonProperty("error_msg")]
    public string? ErrorMessage { get; set; }
}
