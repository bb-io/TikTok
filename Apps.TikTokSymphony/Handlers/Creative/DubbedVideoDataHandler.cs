using Apps.TikTokSymphony.Models.Dtos;
using Apps.TikTokSymphony.Models.Responses.Creative;
using Apps.TikTokSymphony.Utils;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.TikTokSymphony.Handlers.Creative;

public class DubbedVideoDataHandler(InvocationContext invocationContext)
    : Invocable(invocationContext, true), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var request = new RestRequest("creative/aigc/video/list/")
            .AddQueryParameter("aigc_video_types", "[\"DUBBING\"]");
        var response = await Client.ExecuteWithErrorHandling<ApiEnvelope<PageDto<CreativeVideoResponse>>>(request);
        var matches = response.Data.Items.Where(x => !string.IsNullOrWhiteSpace(x.VideoId)
            && CreativeCatalogue.Matches(context.SearchString, x.Name, x.VideoId, x.DubbingInfo?.TargetLanguage))
            .DistinctBy(x => x.VideoId).Take(101).ToList();

        if (matches.Count > 100 || response.Data.Items.Count > 5000)
            throw new PluginMisconfigurationException("The dubbed-video catalogue is too broad. Enter a more specific name or video ID, or map a known ID. This provider contract has no pagination controls.");

        return matches.Select(x => new DataSourceItem(x.VideoId!, $"{x.Name} · {x.DubbingInfo?.TargetLanguage} ({x.VideoId})")).ToList();
    }
}
