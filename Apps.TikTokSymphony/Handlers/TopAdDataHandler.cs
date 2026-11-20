using Apps.TikTokSymphony.Extensions;
using Apps.TikTokSymphony.Models.Responses;
using Apps.TikTokSymphony.Models.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.TikTokSymphony.Handlers;

public class TopAdDataHandler(InvocationContext invocationContext,
    [ActionParameter] InspirationSelection input)
    : Invocable(invocationContext), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(input.TemplateId) || !string.IsNullOrWhiteSpace(input.InspirationVideoUrl))
            return [];

        var filtering = new Dictionary<string, object>()
            .AddOptional("search_keyword", context.SearchString?.Trim());

        return await Client.SearchSuggestions<TopAdResponse>((page, size) =>
            new RestRequest("symphony/trend/top_ad/get/")
                .AddQueryParameter("tto_id", Client.TtoId)
                .AddQueryParameter("filtering", JsonConvert.SerializeObject(filtering))
                .AddQueryParameter("page", page)
                .AddQueryParameter("page_size", size),
            x => x.TopAdId,
            x => $"{string.Join(" · ", new[] { x.Name, x.Brand, x.Region }.Where(v => !string.IsNullOrWhiteSpace(v)))} — {x.TopAdId}",
            _ => true,
            "Narrow the ad name or keyword search.");
    }
}
