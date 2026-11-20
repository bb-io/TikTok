using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Apps.TikTokSymphony.Handlers;
using Apps.TikTokSymphony.Handlers.Static;
using Apps.TikTokSymphony.Models.Responses;
using Apps.TikTokSymphony.Models.Identifiers;

namespace Apps.TikTokSymphony.Models.Identifiers;

public class TaskIdentifier
{
    [Display("Video task ID", Description = "Search tasks from this connection by ID, type, model, or status. Only the last seven days are available; enter or map an ID directly.")]
    [DataSource(typeof(TaskDataHandler))]
    public virtual string TaskId { get; set; } = string.Empty;
}
