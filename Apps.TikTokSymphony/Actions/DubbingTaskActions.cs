using Apps.TikTokSymphony.Extensions;
using Apps.TikTokSymphony.Models.Dtos;
using Apps.TikTokSymphony.Models.Identifiers.Creative;
using Apps.TikTokSymphony.Models.Requests.Creative;
using Apps.TikTokSymphony.Models.Responses;
using Apps.TikTokSymphony.Models.Responses.Creative;
using Apps.TikTokSymphony.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.TikTokSymphony.Actions;

[ActionList("Dubbing tasks and videos")]
public class DubbingTaskActions(InvocationContext invocationContext) : Invocable(invocationContext, true)
{
    [Action("Get dubbing task", Description = "Get the current result of a dubbing task. SUCCESS/FAILED are terminal; video and subtitle URLs have separate expiry periods.")]
    public async Task<CreativeTaskResponse> GetDubbingTask([ActionParameter] DubbingTaskIdentifier input)
    {
        InputValidator.Required(input.TaskId, "Dubbing task ID");
        var request = new RestRequest("creative/aigc/video/task/list/")
            .AddQueryParameter("task_ids", JsonConvert.SerializeObject(new[] { input.TaskId }))
            .AddQueryParameter("aigc_video_type", "DUBBING");
        var response = await Client.ExecuteWithErrorHandling<ApiEnvelope<PageDto<CreativeTaskResponse>>>(request);

        return response.Data.Items.FirstOrDefault()
            ?? throw new PluginMisconfigurationException("The dubbing task was not found. Check its task ID and the authorized creative-tools account.");
    }

    [Action("Search dubbed videos", Description = "Search finished dubbed videos returned for the authorized account. Text and target-language filters are applied locally; the supplied provider contract has no pagination controls.")]
    public async Task<SearchDubbedVideosResponse> SearchDubbedVideos([ActionParameter] SearchDubbedVideosRequest input)
    {
        InputValidator.MaximumResults(input.MaximumResults);
        var request = new RestRequest("creative/aigc/video/list/")
            .AddQueryParameter("aigc_video_types", JsonConvert.SerializeObject(new[] { "DUBBING" }));
        var response = await Client.ExecuteWithErrorHandling<ApiEnvelope<PageDto<CreativeVideoResponse>>>(request);

        if (response.Data.Items.Count > 5000)
            throw new PluginMisconfigurationException("TikTok returned more than 5,000 dubbed videos in one response. Use a known task ID with Get dubbing task; this API contract has no documented paging or server-side search.");

        var videos = response.Data.Items.Where(x =>
            CreativeCatalogue.Matches(input.SearchText, x.Name, x.VideoId)
            && (input.TargetLanguage is null || x.DubbingInfo?.TargetLanguage == input.TargetLanguage));

        return new() { Videos = videos.Take(input.MaximumResults ?? int.MaxValue).ToList() };
    }

    [Action("Get dubbed video", Description = "Get a finished dubbed video's metadata and original/translated script segments. Select an accessible video ID or enter/map one directly.")]
    public async Task<CreativeVideoResponse> GetDubbedVideo([ActionParameter] DubbedVideoIdentifier input)
    {
        InputValidator.Required(input.VideoId, "Dubbed video ID");
        var response = await SearchDubbedVideos(new());

        return response.Videos.FirstOrDefault(x => x.VideoId == input.VideoId)
            ?? throw new PluginMisconfigurationException("The dubbed video was not returned for this connection. Check the video ID; it differs from the task ID.");
    }

    [Action("Download dubbed video", Description = "Get a succeeded dubbing task's video as a native file. Preview URLs expire after 180 days and cannot be renewed by re-querying.")]
    public async Task<FileResponse> DownloadDubbedVideo([ActionParameter] DubbingTaskIdentifier input)
    {
        var task = await GetDubbingTask(input);
        return CreativeFiles.FromTask(task, false);
    }

    [Action("Download dubbed subtitles", Description = "Get a succeeded dubbing task's SRT file. Subtitle links expire after seven days and cannot be renewed by re-querying.")]
    public async Task<FileResponse> DownloadDubbedSubtitles([ActionParameter] DubbingTaskIdentifier input)
    {
        var task = await GetDubbingTask(input);
        return CreativeFiles.FromTask(task, true);
    }

}
