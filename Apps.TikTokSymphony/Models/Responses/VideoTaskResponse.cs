using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Apps.TikTokSymphony.Handlers;
using Apps.TikTokSymphony.Handlers.Static;
using Apps.TikTokSymphony.Models.Responses;
using Apps.TikTokSymphony.Models.Identifiers;

namespace Apps.TikTokSymphony.Models.Responses;

public class VideoTaskResponse
{
    [Display("Video task ID")]
    [JsonProperty("task_id")]
    public string TaskId { get; set; } = string.Empty;

    [Display("Status")]
    [JsonProperty("task_status")]
    public string Status { get; set; } = string.Empty;

    [Display("Task type")]
    [JsonProperty("task_type")]
    public string TaskType { get; set; } = string.Empty;

    [Display("Model")]
    [JsonProperty("model")]
    public string? Model { get; set; }

    [Display("Payment method")]
    [JsonProperty("payment_method")]
    public string? PaymentMethod { get; set; }

    [Display("Created at")]
    [JsonProperty("create_time")]
    public DateTime? CreatedAt { get; set; }

    [Display("Updated at")]
    [JsonProperty("update_time")]
    public DateTime? UpdatedAt { get; set; }

    [Display("Video")]
    [JsonProperty("video")]
    public VideoResultResponse? Video { get; set; }

    [Display("Error")]
    [JsonProperty("error")]
    public TaskErrorResponse? Error { get; set; }

    [Display("Usage")]
    [JsonProperty("usage")]
    public UsageResponse? Usage { get; set; }

    [Display("Settings")]
    [JsonProperty("settings")]
    public TaskSettingsResponse? Settings { get; set; }
}
