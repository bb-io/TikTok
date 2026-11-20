using Apps.TikTokSymphony.Extensions;
using Apps.TikTokSymphony.Models.Responses;
using Apps.TikTokSymphony.Models.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.TikTokSymphony.Handlers;

public class TemplateDataHandler(InvocationContext invocationContext,
    [ActionParameter] InspirationSelection input)
    : Invocable(invocationContext), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(input.TopAdId) || !string.IsNullOrWhiteSpace(input.InspirationVideoUrl))
            return [];

        var filtering = new Dictionary<string, object>()
            .AddOptional("search_keyword", context.SearchString?.Trim());

        return await Client.SearchSuggestions<TemplateResponse>((page, size) =>
            new RestRequest("symphony/trend/template/get/")
                .AddQueryParameter("tto_id", Client.TtoId)
                .AddQueryParameter("filtering", JsonConvert.SerializeObject(filtering))
                .AddQueryParameter("page", page)
                .AddQueryParameter("page_size", size),
            x => x.TemplateId,
            x => $"{x.Name} — {x.TemplateId}",
            _ => true,
            "Narrow the template name or keyword search.");
    }
}
