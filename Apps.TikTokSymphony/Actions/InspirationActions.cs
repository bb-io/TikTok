using Apps.TikTokSymphony.Extensions;
using Apps.TikTokSymphony.Models.Requests;
using Apps.TikTokSymphony.Models.Responses;
using Apps.TikTokSymphony.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.TikTokSymphony.Actions;

[ActionList("Creative inspiration")]
public class InspirationActions(InvocationContext invocationContext) : Invocable(invocationContext)
{
    [Action("Search templates", Description = "Find curated video templates for ad video generation.")]
    public async Task<SearchTemplatesResponse> SearchTemplates([ActionParameter] SearchInspirationRequest input)
    {
        InputValidator.MaximumResults(input.MaximumResults);
        var filtering = new Dictionary<string, object>()
            .AddOptional("search_keyword", input.Search)
            .AddOptional("industry", input.Industry is null ? null : new[] { input.Industry });
        var templates = await Client.Paginate<TemplateResponse>((page, size) =>
            new RestRequest("symphony/trend/template/get/")
                .AddQueryParameter("tto_id", Client.TtoId)
                .AddQueryParameter("filtering", JsonConvert.SerializeObject(filtering))
                .AddQueryParameter("page", page)
                .AddQueryParameter("page_size", size), input.MaximumResults ?? 100);

        return new() { Templates = templates };
    }

    [Action("Search top ads", Description = "Find top-performing ads to use as creative inspiration.")]
    public async Task<SearchTopAdsResponse> SearchTopAds([ActionParameter] SearchTopAdsRequest input)
    {
        InputValidator.MaximumResults(input.MaximumResults);
        var filtering = new Dictionary<string, object>()
            .AddOptional("search_keyword", input.Search)
            .AddOptional("industry", input.Industry is null ? null : new[] { input.Industry })
            .AddOptional("region", input.Region is null ? null : new[] { input.Region })
            .AddOptional("objective", input.Objective is null ? null : new[] { input.Objective });
        var ads = await Client.Paginate<TopAdResponse>((page, size) =>
            new RestRequest("symphony/trend/top_ad/get/")
                .AddQueryParameter("tto_id", Client.TtoId)
                .AddQueryParameter("filtering", JsonConvert.SerializeObject(filtering))
                .AddQueryParameter("page", page)
                .AddQueryParameter("page_size", size), input.MaximumResults ?? 100);

        return new() { TopAds = ads };
    }
}
