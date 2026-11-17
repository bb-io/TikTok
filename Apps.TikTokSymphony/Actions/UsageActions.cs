using Apps.TikTokSymphony.Models.Requests;
using Apps.TikTokSymphony.Models.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.TikTokSymphony.Actions;

[ActionList("Generation usage")]
public class UsageActions(InvocationContext invocationContext) : Invocable(invocationContext)
{
    [Action("Get generation usage", Description = "Summarize statuses, credits, tokens, and video duration for retained tasks by creation date. Covers at most seven days; excludes deleted records and is not an invoice or audience report.")]
    public async Task<GenerationUsageResponse> GetGenerationUsage([ActionParameter] GenerationUsageRequest input)
    {
        var earliest = DateTime.UtcNow.AddDays(-7);

        if (Utc(input.CreatedAfter) < earliest || Utc(input.CreatedBefore) < earliest)
            throw new PluginMisconfigurationException("Choose task creation dates within the last seven days. TikTok does not retain older Symphony task records.");

        var response = await new TaskActions(InvocationContext).SearchVideoTasks(new()
        {
            CreatedAfter = input.CreatedAfter,
            CreatedBefore = input.CreatedBefore,
            TaskType = input.TaskType,
            MaximumResults = 1001
        });

        if (response.Tasks.Count > 1000)
            throw new PluginMisconfigurationException("More than 1,000 tasks match. Narrow the creation date range or task type to get a complete usage summary.");

        var tasks = response.Tasks.DistinctBy(x => x.TaskId).ToList();

        return new()
        {
            TotalTasks = tasks.Count,
            SucceededTasks = tasks.Count(x => x.Status == "SUCCEEDED"),
            FailedTasks = tasks.Count(x => x.Status == "FAILED"),
            ExpiredTasks = tasks.Count(x => x.Status == "EXPIRED"),
            CanceledTasks = tasks.Count(x => x.Status == "CANCELLED"),
            QueuedTasks = tasks.Count(x => x.Status == "QUEUED"),
            RunningTasks = tasks.Count(x => x.Status == "RUNNING"),
            BonusCreditsUsed = tasks.Sum(x => x.Usage?.BonusCreditsUsed ?? 0),
            PaidTokensUsed = tasks.Sum(x => x.Usage?.PaidTokensUsed ?? 0),
            GeneratedDurationSeconds = tasks.Where(x => x.Status == "SUCCEEDED").Sum(x => x.Video?.Duration ?? 0)
        };
    }

    private static DateTime? Utc(DateTime? value) => value is null ? null
        : value.Value.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc)
            : value.Value.ToUniversalTime();
}
