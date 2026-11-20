using System.Security.Cryptography;
using System.Text;
using Apps.TikTokSymphony.Actions;
using Apps.TikTokSymphony.Constants;
using Apps.TikTokSymphony.Models.Identifiers.Creative;
using Apps.TikTokSymphony.Models.Responses.Creative;
using Apps.TikTokSymphony.Utils;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Polling;

namespace Apps.TikTokSymphony.Polling;

[PollingEventList]
public class CreativePollingList(InvocationContext invocationContext) : Invocable(invocationContext, true)
{
    [PollingEvent("On dubbing task finished", Description = "Continue when a selected dubbing task reaches SUCCESS or FAILED. First poll establishes a baseline; check Status before downloading.")]
    public Task<PollingEventResponse<CreativeCompletionMemory, CreativeTaskResponse>> OnDubbingTaskFinished(
        PollingEventRequest<CreativeCompletionMemory> request,
        [PollingEventParameter] DubbingTaskIdentifier input) => Poll(request, input.TaskId, "DUBBING");

    [PollingEvent("On avatar task finished", Description = "Continue when a selected avatar task reaches SUCCESS or FAILED. First poll establishes a baseline; check Status before downloading.")]
    public Task<PollingEventResponse<CreativeCompletionMemory, CreativeTaskResponse>> OnAvatarTaskFinished(
        PollingEventRequest<CreativeCompletionMemory> request,
        [PollingEventParameter] AvatarTaskIdentifier input) => Poll(request, input.TaskId, "AVATAR");

    private async Task<PollingEventResponse<CreativeCompletionMemory, CreativeTaskResponse>> Poll(
        PollingEventRequest<CreativeCompletionMemory> request, string taskId, string kind)
    {
        InputValidator.Required(taskId, "Creative task ID");
        var token = InvocationContext.AuthenticationCredentialsProviders.First(x => x.KeyName == CredsNames.AccessToken).Value;
        var connectionKey = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
        var memory = request.Memory;

        if (memory is null || memory.TaskId != taskId || memory.Kind != kind || memory.ConnectionKey != connectionKey)
        {
            return new()
            {
                FlyBird = false,
                Memory = new CreativeCompletionMemory { TaskId = taskId, Kind = kind, ConnectionKey = connectionKey },
                Result = null
            };
        }

        if (memory.Triggered)
            return new() { FlyBird = false, Memory = memory, Result = null };

        CreativeTaskResponse task = kind == "DUBBING"
            ? await new DubbingTaskActions(InvocationContext).GetDubbingTask(new() { TaskId = taskId })
            : await new AvatarActions(InvocationContext).GetAvatarTask(new() { TaskId = taskId });
        var terminal = task.Status is "SUCCESS" or "FAILED";

        return new()
        {
            FlyBird = terminal,
            Memory = new CreativeCompletionMemory
            {
                TaskId = taskId,
                ConnectionKey = connectionKey,
                Kind = kind,
                Triggered = terminal
            },
            Result = terminal ? task : null
        };
    }
}
