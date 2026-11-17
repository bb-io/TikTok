using Apps.TikTokSymphony.Models.Requests.Creative;
using Apps.TikTokSymphony.Models.Responses.Creative;
using Apps.TikTokSymphony.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.TikTokSymphony.Actions;

[ActionList("Avatars and voices")]
public class CreativeCatalogueActions(InvocationContext invocationContext) : Invocable(invocationContext, true)
{
    [Action("Search digital avatars", Description = "Find available avatars by name, tags, ID and optional avatar type. Requires TikTok creative-tools permission and allowlisting.")]
    public async Task<SearchAvatarsResponse> SearchDigitalAvatars([ActionParameter] AvatarSearchRequest input)
    {
        InputValidator.MaximumResults(input.MaximumResults);
        var avatars = await Client.Paginate<AvatarResponse>((page, size) =>
        {
            var request = new RestRequest("creative/digital_avatar/get/")
                .AddQueryParameter("page", page).AddQueryParameter("page_size", size);

            if (!string.IsNullOrWhiteSpace(input.Identity))
                request.AddQueryParameter("tag_groups", JsonConvert.SerializeObject(new[]
                {
                    new { tag_type = "identity", tags = new[] { input.Identity } }
                }));

            return request;
        }, input.MaximumResults, x => CreativeCatalogue.AvatarMatches(x, input.SearchText), 50);

        return new() { Avatars = avatars };
    }

    [Action("Search voices", Description = "Find current voices by name, tags or ID. Results include voice metadata and preview audio URLs; language suitability must be verified from provider tags.")]
    public async Task<SearchVoicesResponse> SearchVoices([ActionParameter] VoiceSearchRequest input)
    {
        InputValidator.MaximumResults(input.MaximumResults);
        var voices = await Client.Paginate<VoiceResponse>((page, size) =>
            new RestRequest("creative/aigc/voice/get/")
                .AddQueryParameter("page", page).AddQueryParameter("page_size", size),
            input.MaximumResults, x => CreativeCatalogue.VoiceMatches(x, input.SearchText), 50);

        return new() { Voices = voices };
    }
}
