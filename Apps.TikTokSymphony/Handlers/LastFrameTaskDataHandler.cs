using Apps.TikTokSymphony.Models.Requests;
using Apps.TikTokSymphony.Models.Responses;
using Apps.TikTokSymphony.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.TikTokSymphony.Handlers;

public class LastFrameTaskDataHandler(InvocationContext invocationContext)
    : Invocable(invocationContext), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        var filtering = TaskSuggestions.Filters(null, new[] { "SUCCEEDED" });

        return await Client.SearchSuggestions<VideoTaskResponse>((page, size) =>
            new RestRequest("symphony/task/get/")
                .AddQueryParameter("tto_id", Client.TtoId)
                .AddQueryParameter("filtering", JsonConvert.SerializeObject(filtering))
                .AddQueryParameter("page", page)
                .AddQueryParameter("page_size", size),
            x => x.TaskId,
            TaskSuggestions.Label,
            x => !string.IsNullOrWhiteSpace(x.Video?.LastFrameUrl) && TaskSuggestions.Matches(x, context.SearchString),
            "Narrow the search by task ID, status, model, or date; the Search video tasks action supports more filters.");
    }
}
