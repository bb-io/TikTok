using Apps.TikTokSymphony.Models.Responses.Creative;
using Apps.TikTokSymphony.Utils;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.TikTokSymphony.Handlers.Creative;

public class VoiceDataHandler(InvocationContext invocationContext)
    : Invocable(invocationContext, true), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken) =>
        await Client.SearchSuggestions<VoiceResponse>((page, size) =>
            new RestRequest("creative/aigc/voice/get/")
                .AddQueryParameter("page", page).AddQueryParameter("page_size", size),
            x => x.VoiceId, CreativeCatalogue.VoiceLabel,
            x => CreativeCatalogue.VoiceMatches(x, context.SearchString),
            "Type a more specific name, tag or ID, or use the Search action.", 50);
}
