using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.TikTokSymphony.Models.Responses.Creative;

public class AvatarCreatedTasksResponse
{
    [Display("Avatar tasks")]
    [JsonProperty("list")]
    public List<AvatarCreatedTaskResponse> Tasks { get; set; } = [];
}
