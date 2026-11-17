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

public class SearchTasksRequest : TaskSearchContext
{
    [Display("Video task IDs", Description = "Suggestions respect status, task type, and date filters on this connection. Enter or map up to 100 IDs directly.")]
    [DataSource(typeof(SearchTaskDataHandler))]
    public List<string>? TaskIds { get; set; }

    [Display("Maximum results", Description = "Leave empty to retrieve all matching tasks from the last seven days.")]
    public int? MaximumResults { get; set; }
}
