using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Apps.TikTokSymphony.Handlers;
using Apps.TikTokSymphony.Handlers.Static;
using Apps.TikTokSymphony.Models.Responses;
using Apps.TikTokSymphony.Models.Identifiers;

namespace Apps.TikTokSymphony.Models.Requests;

public class DeleteTasksRequest
{
    [Display("Video task IDs", Description = "Suggestions include queued or finished tasks on this connection. Queued tasks are canceled; finished records are permanently deleted. You can enter or map IDs directly.")]
    [DataSource(typeof(DeletableTaskDataHandler))]
    public List<string> TaskIds { get; set; } = [];
}
