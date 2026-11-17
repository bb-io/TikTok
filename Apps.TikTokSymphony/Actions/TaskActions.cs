using System.Globalization;
using Apps.TikTokSymphony.Extensions;
using Apps.TikTokSymphony.Models.Dtos;
using Apps.TikTokSymphony.Models.Identifiers;
using Apps.TikTokSymphony.Models.Requests;
using Apps.TikTokSymphony.Models.Responses;
using Apps.TikTokSymphony.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.TikTokSymphony.Actions;

[ActionList("Video tasks")]
public class TaskActions(InvocationContext invocationContext) : Invocable(invocationContext)
{
    [Action("Get video task", Description = "Get the current status, result URLs, and usage for a video task.")]
    public async Task<VideoTaskResponse> GetVideoTask([ActionParameter] TaskIdentifier input)
    {
        InputValidator.Required(input.TaskId, "Video task ID");
        var tasks = await SearchVideoTasks(new SearchTasksRequest { TaskIds = [input.TaskId], MaximumResults = 1 });

        return tasks.Tasks.FirstOrDefault()
            ?? throw new PluginMisconfigurationException(
                "The video task was not found. Check the task ID and TTO account. Only tasks from the last seven days can be retrieved.");
    }

    [Action("Search video tasks", Description = "Find recent video tasks. Only tasks created in the last seven days are available.")]
    public async Task<SearchTasksResponse> SearchVideoTasks([ActionParameter] SearchTasksRequest input)
    {
        InputValidator.MaximumResults(input.MaximumResults);

        if (input.TaskIds is not null)
        {
            InputValidator.TaskIds(input.TaskIds);

            if (input.TaskIds.Count > 100)
                throw new PluginMisconfigurationException("Provide no more than 100 video task IDs.");
        }

        if (input.CreatedAfter > input.CreatedBefore)
            throw new PluginMisconfigurationException("Set 'Created after' before 'Created before'.");

        var filtering = new Dictionary<string, object>()
            .AddOptional("task_ids", input.TaskIds)
            .AddOptional("task_statuses", input.Status is null ? null : new[] { input.Status })
            .AddOptional("task_types", input.TaskType is null ? null : new[] { input.TaskType })
            .AddOptional("create_start_time", UtcTime(input.CreatedAfter))
            .AddOptional("create_end_time", UtcTime(input.CreatedBefore));
        var tasks = await Client.Paginate<VideoTaskResponse>((page, size) =>
            new RestRequest("symphony/task/get/")
                .AddQueryParameter("tto_id", Client.TtoId)
                .AddQueryParameter("filtering", JsonConvert.SerializeObject(filtering))
                .AddQueryParameter("page", page)
                .AddQueryParameter("page_size", size), input.MaximumResults);

        return new() { Tasks = tasks };
    }

    [Action("Cancel or delete video tasks", Description = "Cancel queued tasks or permanently delete finished task records. Running tasks cannot be canceled.")]
    public async Task<DeleteTasksResponse> CancelOrDeleteVideoTasks([ActionParameter] DeleteTasksRequest input)
    {
        InputValidator.TaskIds(input.TaskIds);
        var request = new RestRequest("symphony/task/delete/", Method.Post)
            .AddJsonBody(new { tto_id = Client.TtoId, task_ids = input.TaskIds });
        var response = await Client.ExecuteWithErrorHandling<ApiEnvelope<DeleteTasksResponse>>(request);

        return response.Data;
    }

    [Action("Download generated video", Description = "Get a completed video as a file. Refreshes the download URL before returning it.")]
    public async Task<FileResponse> DownloadGeneratedVideo([ActionParameter] DownloadTaskIdentifier input)
    {
        var task = await GetVideoTask(input);
        RequireSuccess(task);
        var url = task.Video?.Url;
        InputValidator.HttpsUrl(url, "Generated video URL");
        var isMov = string.Equals(task.Settings?.OutputFormat, "MOV", StringComparison.OrdinalIgnoreCase);
        var extension = isMov ? "mov" : "mp4";

        return new()
        {
            File = new FileReference(new HttpRequestMessage(HttpMethod.Get, url),
                $"{SafeFileName(task.TaskId)}.{extension}", isMov ? "video/quicktime" : "video/mp4")
        };
    }

    [Action("Download generated videos", Description = "Get files for up to 100 succeeded tasks. Refreshes URLs in one batch; fails if any requested task is missing or unfinished.")]
    public async Task<DownloadedVideosResponse> DownloadGeneratedVideos([ActionParameter] DownloadVideosRequest input)
    {
        InputValidator.TaskIds(input.TaskIds);
        var ids = input.TaskIds.Distinct(StringComparer.Ordinal).ToList();

        if (input.TaskIds.Count > 100)
            throw new PluginMisconfigurationException("Provide no more than 100 video task IDs per batch.");

        var response = await SearchVideoTasks(new SearchTasksRequest { TaskIds = ids });
        var byId = response.Tasks.DistinctBy(x => x.TaskId).ToDictionary(x => x.TaskId, StringComparer.Ordinal);
        var missing = ids.Where(x => !byId.ContainsKey(x)).ToList();

        if (missing.Count > 0)
            throw new PluginMisconfigurationException("Some video tasks were not found. Check the connection and seven-day retention window. Missing task IDs: " + string.Join(", ", missing));

        var output = new DownloadedVideosResponse();

        foreach (var id in ids)
        {
            var task = byId[id];
            RequireSuccess(task);
            InputValidator.HttpsUrl(task.Video?.Url, "Generated video URL");
            var isMov = string.Equals(task.Settings?.OutputFormat, "MOV", StringComparison.OrdinalIgnoreCase);
            output.Videos.Add(new DownloadedVideoResponse
            {
                TaskId = id,
                File = new FileReference(new HttpRequestMessage(HttpMethod.Get, task.Video!.Url),
                    $"{SafeFileName(id)}.{(isMov ? "mov" : "mp4")}", isMov ? "video/quicktime" : "video/mp4")
            });
        }

        return output;
    }

    [Action("Download last frame", Description = "Get the last frame as a JPEG file when the generation task requested it.")]
    public async Task<FileResponse> DownloadLastFrame([ActionParameter] LastFrameTaskIdentifier input)
    {
        var task = await GetVideoTask(input);
        RequireSuccess(task);

        if (string.IsNullOrWhiteSpace(task.Video?.LastFrameUrl))
            throw new PluginMisconfigurationException("No last frame is available. Enable 'Return last frame' when creating a video with paid tokens.");

        InputValidator.HttpsUrl(task.Video.LastFrameUrl, "Last frame URL");

        return new()
        {
            File = new FileReference(new HttpRequestMessage(HttpMethod.Get, task.Video.LastFrameUrl),
                $"{SafeFileName(task.TaskId)}-last-frame.jpg", "image/jpeg")
        };
    }

    private static void RequireSuccess(VideoTaskResponse task)
    {
        if (task.Status != "SUCCEEDED")
            throw new PluginMisconfigurationException(
                $"The video task is {task.Status.ToLowerInvariant()}. Download is available only after it succeeds. {task.Error?.Message}");
    }

    private static string SafeFileName(string value) =>
        string.Concat(value.Select(x => char.IsLetterOrDigit(x) || x is '-' or '_' ? x : '_'));

    private static string? UtcTime(DateTime? value) => value is null
        ? null
        : (value.Value.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc)
            : value.Value.ToUniversalTime()).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
}
