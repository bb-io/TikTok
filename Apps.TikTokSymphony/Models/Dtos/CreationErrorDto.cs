using Apps.TikTokSymphony.Models.Responses;
using Apps.TikTokSymphony.Models.Responses.Creative;
using Newtonsoft.Json;

namespace Apps.TikTokSymphony.Models.Dtos;

public class CreationErrorDto
{
    [JsonProperty("tasks")]
    public List<VideoTaskResponse> Tasks { get; set; } = [];

    [JsonProperty("task_ids")]
    public List<string> TaskIds { get; set; } = [];

    [JsonProperty("list")]
    public List<AvatarCreatedTaskResponse> Items { get; set; } = [];
}
