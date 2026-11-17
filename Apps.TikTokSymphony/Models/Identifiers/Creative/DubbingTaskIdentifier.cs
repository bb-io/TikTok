using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.TikTokSymphony.Models.Identifiers.Creative;

public class DubbingTaskIdentifier
{
    [Display("Dubbing task ID", Description = "Map from Translate and dub video or reviewed dubbing. Pending task IDs cannot be enumerated by the documented API.")]
    public string TaskId { get; set; } = string.Empty;
}
