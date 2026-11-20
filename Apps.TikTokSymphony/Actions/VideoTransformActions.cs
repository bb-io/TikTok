using Apps.TikTokSymphony.Extensions;
using Apps.TikTokSymphony.Models.Dtos;
using Apps.TikTokSymphony.Models.Requests;
using Apps.TikTokSymphony.Models.Responses;
using Apps.TikTokSymphony.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.TikTokSymphony.Actions;

[ActionList("Video editing")]
public class VideoTransformActions(InvocationContext invocationContext) : Invocable(invocationContext)
{
    [Action("Edit video", Description = "Create an edited version of a video using a prompt. Requires Seedance 2.5 and paid tokens; returns a generation task ID.")]
    public Task<CreatedTasksResponse> EditVideo([ActionParameter] EditVideoRequest input)
    {
        if (input.PaymentMethod != "PAID_TOKENS")
            throw new PluginMisconfigurationException("Select BytePlus paid tokens for video editing. Bonus credits do not support editing.");

        return CreateTask(input, "EDIT", -1);
    }

    [Action("Extend video", Description = "Generate a continuation of a video with Seedance 2.5. Consumes credits or paid tokens; TikTok allows at most two extensions per video.")]
    public Task<CreatedTasksResponse> ExtendVideo([ActionParameter] ExtendVideoRequest input)
    {
        if (input.Duration.HasValue && !(input.Duration == -1 && input.PaymentMethod == "PAID_TOKENS")
            && (input.Duration < 4 || input.Duration > 30))
            throw new PluginMisconfigurationException("Set 'Duration in seconds' between 4 and 30, or -1 with paid tokens.");

        return CreateTask(input, "EXTEND", input.Duration);
    }

    private async Task<CreatedTasksResponse> CreateTask(VideoTransformRequest input, string mode, int? duration)
    {
        InputValidator.Required(input.Prompt, "Prompt");
        InputValidator.HttpsUrl(input.VideoUrl, "Source video URL");

        if (input.PaymentMethod is not "PAID_TOKENS" and not "BONUS_CREDITS")
            throw new PluginMisconfigurationException("Choose a payment method. The task consumes credits or paid tokens.");

        if (input.Resolution is not null && input.Resolution is not "480P" and not "720P" and not "1080P")
            throw new PluginMisconfigurationException("Choose 480P, 720P, or 1080P resolution for Seedance 2.5.");

        if (input.PaymentMethod == "BONUS_CREDITS"
            && (input.Resolution is not null || input.GenerateAudio.HasValue || input.ReturnLastFrame.HasValue))
            throw new PluginMisconfigurationException("Leave 'Resolution', 'Generate audio', and 'Return last frame' empty with bonus credits. Seedance 2.5 bonus-credit access requires TikTok allowlisting.");

        var content = new object[]
        {
            new { type = "TEXT", text = input.Prompt },
            new { type = "VIDEO", video = new { url = input.VideoUrl }, role = "REFERENCE_VIDEO" }
        };
        var body = new Dictionary<string, object>
        {
            ["tto_id"] = Client.TtoId,
            ["payment_method"] = input.PaymentMethod,
            ["model"] = "SEEDANCE_2_5",
            ["omni_reference_task_type"] = mode,
            ["content"] = content
        }
        .AddOptional("duration", duration)
        .AddOptional("resolution", input.Resolution)
        .AddOptional("generate_audio", input.GenerateAudio)
        .AddOptional("return_last_frame", input.ReturnLastFrame);

        if (input.PaymentMethod == "PAID_TOKENS")
            body["ratio"] = "ADAPTIVE";

        var request = new RestRequest("symphony/video/task/create/", Method.Post).AddJsonBody(body);
        var response = await Client.ExecuteWithErrorHandling<ApiEnvelope<CreatedTasksResponse>>(request);

        return response.Data;
    }
}
