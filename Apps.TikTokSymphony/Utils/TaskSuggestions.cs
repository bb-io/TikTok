using System.Globalization;
using Apps.TikTokSymphony.Extensions;
using Apps.TikTokSymphony.Models.Requests;
using Apps.TikTokSymphony.Models.Responses;

namespace Apps.TikTokSymphony.Utils;

public static class TaskSuggestions
{
    public static Dictionary<string, object> Filters(TaskSearchContext? input, string[]? statuses = null) =>
        new Dictionary<string, object>()
            .AddOptional("task_statuses", statuses ?? (input?.Status is null ? null : new[] { input.Status }))
            .AddOptional("task_types", input?.TaskType is null ? null : new[] { input.TaskType })
            .AddOptional("create_start_time", UtcTime(input?.CreatedAfter))
            .AddOptional("create_end_time", UtcTime(input?.CreatedBefore));

    public static string Label(VideoTaskResponse task)
    {
        var type = task.TaskType == "INSIGHT_TO_VIDEO" ? "Ad video" : "Video";
        var created = task.CreatedAt?.ToString("yyyy-MM-dd HH:mm 'UTC'", CultureInfo.InvariantCulture);
        var model = task.Model switch
        {
            "SEEDANCE_2_0" => "Seedance 2.0",
            "SEEDANCE_2_0_FAST" => "Seedance 2.0 Fast",
            "SEEDANCE_2_0_MINI" => "Seedance 2.0 Mini",
            "SEEDANCE_2_5" => "Seedance 2.5",
            _ => task.Model
        };
        var details = string.Join(" · ", new[] { type, task.Status, model, created }.Where(x => !string.IsNullOrWhiteSpace(x)));

        return $"{details} — {task.TaskId}";
    }

    public static bool Matches(VideoTaskResponse task, string? search) =>
        string.IsNullOrWhiteSpace(search)
        || Label(task).Contains(search.Trim(), StringComparison.OrdinalIgnoreCase);

    private static string? UtcTime(DateTime? value) => value is null
        ? null
        : (value.Value.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc)
            : value.Value.ToUniversalTime()).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
}
