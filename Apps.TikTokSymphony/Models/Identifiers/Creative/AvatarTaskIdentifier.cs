using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.TikTokSymphony.Models.Identifiers.Creative;

public class AvatarTaskIdentifier
{
    [Display("Avatar task ID", Description = "Map from Generate avatar video. Pending task IDs cannot be enumerated by the documented API.")]
    public string TaskId { get; set; } = string.Empty;
}
