using Apps.TikTokSymphony.Models.Responses.Creative;
using Apps.TikTokSymphony.Utils;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.TikTokSymphony.Handlers.Creative;

public class AvatarDataHandler(InvocationContext invocationContext)
    : Invocable(invocationContext, true), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken) =>
        await Client.SearchSuggestions<AvatarResponse>((page, size) =>
            new RestRequest("creative/digital_avatar/get/")
                .AddQueryParameter("page", page).AddQueryParameter("page_size", size),
            x => x.AvatarId, CreativeCatalogue.AvatarLabel,
            x => CreativeCatalogue.AvatarMatches(x, context.SearchString),
            "Type a more specific name, tag or ID, or use the Search action.", 50);
}
