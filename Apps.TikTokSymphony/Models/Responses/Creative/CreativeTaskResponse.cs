using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.TikTokSymphony.Models.Responses.Creative;

public class CreativeTaskResponse : CreativeMediaResponse
{
    [Display("Creative task ID")]
    [JsonProperty("task_id")]
    public string TaskId { get; set; } = string.Empty;

    [Display("Status")]
    [JsonProperty("status")]
    public string Status { get; set; } = string.Empty;

    [Display("Error message")]
    [JsonProperty("error_msg")]
    public string? ErrorMessage { get; set; }
}
