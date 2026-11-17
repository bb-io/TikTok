using Apps.TikTokSymphony.Actions;
using Apps.TikTokSymphony.Models.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Polling;

namespace Apps.TikTokSymphony.Polling;

[PollingEventList]
public class AccountTaskPollingList(InvocationContext invocationContext) : Invocable(invocationContext)
{
    [PollingEvent("On generated video ready", Description = "Start one Flight per newly observed succeeded task in the connected TTO account. First poll records a baseline; covers the seven-day task retention window."), MultipleEvents]
    public Task<PollingEventResponse<AccountTaskMemory, List<VideoTaskResponse>>> OnGeneratedVideoReady(
        PollingEventRequest<AccountTaskMemory> request) => Poll(request, "SUCCEEDED");

    [PollingEvent("On video generation failed", Description = "Start one Flight per newly observed failed or expired task in the connected TTO account. First poll records a baseline; canceled tasks are excluded."), MultipleEvents]
    public Task<PollingEventResponse<AccountTaskMemory, List<VideoTaskResponse>>> OnVideoGenerationFailed(
        PollingEventRequest<AccountTaskMemory> request) => Poll(request, "FAILURE");

    private async Task<PollingEventResponse<AccountTaskMemory, List<VideoTaskResponse>>> Poll(
        PollingEventRequest<AccountTaskMemory> request, string outcome)
    {
        var actions = new TaskActions(InvocationContext);
        var snapshot = await actions.SearchVideoTasks(new() { MaximumResults = 1001 });

        if (snapshot.Tasks.Count > 1000)
            throw new PluginMisconfigurationException("Account-wide monitoring is incomplete: more than 1,000 recent tasks are available. Use 'On video task finished' for specific tasks instead. No polling memory was advanced.");

        var matching = snapshot.Tasks.Where(x => outcome == "SUCCEEDED"
            ? x.Status == "SUCCEEDED"
            : x.Status is "FAILED" or "EXPIRED").DistinctBy(x => x.TaskId).ToList();
        var memory = request.Memory;
        var baseline = memory is null || memory.TtoId != Client.TtoId || memory.Outcome != outcome;
        var seen = baseline ? new HashSet<string>(StringComparer.Ordinal)
            : memory!.SeenTaskIds.ToHashSet(StringComparer.Ordinal);
        var result = baseline ? [] : matching.Where(x => !seen.Contains(x.TaskId))
            .OrderBy(x => x.UpdatedAt).ThenBy(x => x.TaskId, StringComparer.Ordinal).Take(100).ToList();

        if (baseline)
            seen.UnionWith(matching.Select(x => x.TaskId));
        else
            seen.UnionWith(result.Select(x => x.TaskId));

        seen.IntersectWith(snapshot.Tasks.Select(x => x.TaskId));

        return new()
        {
            FlyBird = result.Count > 0,
            Memory = new AccountTaskMemory
            {
                TtoId = Client.TtoId,
                Outcome = outcome,
                SeenTaskIds = seen.Order(StringComparer.Ordinal).ToList()
            },
            Result = result
        };
    }
}
