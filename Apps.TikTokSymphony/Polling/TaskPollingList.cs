using Apps.TikTokSymphony.Actions;
using Apps.TikTokSymphony.Models.Identifiers;
using Apps.TikTokSymphony.Models.Responses;
using Apps.TikTokSymphony.Utils;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Polling;

namespace Apps.TikTokSymphony.Polling;

[PollingEventList]
public class TaskPollingList(InvocationContext invocationContext) : Invocable(invocationContext)
{
    [PollingEvent("On video task finished", Description = "Continue once a selected task succeeds, fails, expires, or is canceled. Check the returned status before downloading.")]
    public async Task<PollingEventResponse<CompletionMemory, VideoTaskResponse>> OnVideoTaskFinished(
        PollingEventRequest<CompletionMemory> request,
        [PollingEventParameter] TaskIdentifier input)
    {
        InputValidator.Required(input.TaskId, "Video task ID");
        var memory = request.Memory;

        if (memory is null || memory.TaskId != input.TaskId)
        {
            return new()
            {
                FlyBird = false,
                Memory = new CompletionMemory { TaskId = input.TaskId },
                Result = null
            };
        }

        if (memory.Triggered)
            return new() { FlyBird = false, Memory = memory, Result = null };

        var task = await new TaskActions(InvocationContext).GetVideoTask(input);
        var finished = task.Status is "SUCCEEDED" or "FAILED" or "EXPIRED" or "CANCELLED";

        return new()
        {
            FlyBird = finished,
            Memory = new CompletionMemory { TaskId = input.TaskId, Triggered = finished },
            Result = finished ? task : null
        };
    }
}
