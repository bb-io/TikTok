using System.Globalization;
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
using Newtonsoft.Json;
using RestSharp;

namespace Apps.TikTokSymphony.Actions;

[ActionList("Digital avatar videos")]
public class AvatarActions(InvocationContext invocationContext) : Invocable(invocationContext, true)
{
    [Action("Generate avatar video", Description = "Create a video of a selected digital avatar reading your script. Requires Generative AI Tool > Video Generation permission and TikTok allowlisting.")]
    public async Task<AvatarCreatedTasksResponse> GenerateAvatarVideo([ActionParameter] CreateAvatarRequest input)
    {
        if (!input.RightsConfirmed)
            throw new PluginMisconfigurationException("Review the asset-rights notice and set 'Asset rights confirmed' to true before submitting your script.");

        InputValidator.Required(input.AvatarId, "Avatar ID");
        InputValidator.Required(input.Script, "Script");
        CreativeValidation.Name(input.VideoName);

        if (input.Script.Length > 2000)
            throw new PluginMisconfigurationException("Limit 'Script' to 2,000 characters and remove emojis or unsupported special characters.");

        if (input.Volume.HasValue && (!double.IsFinite(input.Volume.Value) || input.Volume < 0 || input.Volume > 10
            || Math.Round(input.Volume.Value, 2) != input.Volume.Value))
            throw new PluginMisconfigurationException("Set 'Voice volume' between 0 and 10 with at most two decimal places.");

        var item = new Dictionary<string, object>
        {
            ["avatar_id"] = input.AvatarId,
            ["script"] = input.Script
        }
        .AddOptional("voice_id", input.VoiceId)
        .AddOptional("voice_volume", input.Volume)
        .AddOptional("voice_speed", input.Speed)
        .AddOptional("transparent_background_enabled", input.TransparentBackground)
        .AddOptional("video_name", input.VideoName);
        var request = new RestRequest("creative/digital_avatar/video/task/create/", Method.Post)
            .AddJsonBody(new { material_packages = new[] { item } });
        var response = await Client.ExecuteWithErrorHandling<ApiEnvelope<AvatarCreatedTasksResponse>>(request);

        return response.Data;
    }

    [Action("Get avatar task", Description = "Get the current status and results of a digital-avatar task. Inspect each creation result for an error before mapping its task ID.")]
    public async Task<AvatarTaskResponse> GetAvatarTask([ActionParameter] AvatarTaskIdentifier input)
    {
        InputValidator.Required(input.TaskId, "Avatar task ID");
        var request = new RestRequest("creative/digital_avatar/video/task/get/")
            .AddQueryParameter("task_ids", JsonConvert.SerializeObject(new[] { input.TaskId }));
        var response = await Client.ExecuteWithErrorHandling<ApiEnvelope<PageDto<AvatarTaskResponse>>>(request);

        return response.Data.Items.FirstOrDefault()
            ?? throw new PluginMisconfigurationException("The avatar task was not found. Check its task ID and authorized creative-tools account.");
    }

    [Action("Search avatar videos", Description = "Find finished avatar videos by text, avatar ID and UTC creation dates. Returns provider video metadata, not pending task IDs.")]
    public async Task<SearchAvatarVideosResponse> SearchAvatarVideos([ActionParameter] SearchAvatarVideosRequest input)
    {
        InputValidator.MaximumResults(input.MaximumResults);

        if (input.CreatedAfter > input.CreatedBefore)
            throw new PluginMisconfigurationException("Set 'Created after' before 'Created before'.");

        var filtering = new Dictionary<string, object>()
            .AddOptional("avatar_id", input.AvatarId)
            .AddOptional("start_date", Date(input.CreatedAfter))
            .AddOptional("end_date", Date(input.CreatedBefore));
        var videos = await Client.Paginate<AvatarVideoResponse>((page, size) =>
            new RestRequest("creative/digital_avatar/video/list/")
                .AddQueryParameter("filtering", JsonConvert.SerializeObject(filtering))
                .AddQueryParameter("page", page).AddQueryParameter("page_size", size),
            input.MaximumResults, x => CreativeCatalogue.Matches(input.SearchText, x.Name, x.VideoId), 50);

        return new() { Videos = videos };
    }

    [Action("Download avatar video", Description = "Get a succeeded avatar task's video as a native file. Its preview URL expires after 180 days and cannot be renewed by re-querying.")]
    public async Task<FileResponse> DownloadAvatarVideo([ActionParameter] AvatarTaskIdentifier input)
    {
        var task = await GetAvatarTask(input);
        return CreativeFiles.FromTask(task, false);
    }

    [Action("Download avatar subtitles", Description = "Get a succeeded avatar task's SRT file. Subtitle links expire after seven days and cannot be renewed by re-querying.")]
    public async Task<FileResponse> DownloadAvatarSubtitles([ActionParameter] AvatarTaskIdentifier input)
    {
        var task = await GetAvatarTask(input);
        return CreativeFiles.FromTask(task, true);
    }

    private static string? Date(DateTime? value) => value is null ? null
        : (value.Value.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc)
            : value.Value.ToUniversalTime()).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
}
