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

[ActionList("Translation and dubbing")]
public class DubbingActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
    : Invocable(invocationContext, true)
{
    [Action("Translate and dub video", Description = "Automatically translate and dub a video into an explicitly selected language. Requires prior per-video end-user consent, creative-tools permission and TikTok allowlisting.")]
    public Task<CreativeCreatedTasksResponse> TranslateAndDubVideo([ActionParameter] CreateDubbingRequest input)
    {
        Validate(input);
        return Submit(input, null);
    }

    [Action("Dub video with reviewed subtitles", Description = "Dub a video using approved target-language SRT text and timing. Requires prior per-video end-user consent; the SRT language must match Target language.")]
    public async Task<CreativeCreatedTasksResponse> DubVideoWithReviewedSubtitles([ActionParameter] ReviewedDubbingRequest input)
    {
        Validate(input);

        if (input.Subtitles?.Name?.EndsWith(".srt", StringComparison.OrdinalIgnoreCase) != true)
            throw new PluginMisconfigurationException("Provide 'Reviewed subtitles' as a UTF-8 .srt file.");

        var text = await Client.ReadTextFile(() => fileManagementClient.DownloadAsync(input.Subtitles), 2 * 1024 * 1024);
        var segments = ReviewedSubtitles.Parse(text);
        return await Submit(input, segments);
    }

    private async Task<CreativeCreatedTasksResponse> Submit(CreateDubbingRequest input, List<Dictionary<string, object>>? scripts)
    {
        var item = new Dictionary<string, object>
        {
            ["video_url"] = input.VideoUrl,
            ["target_languages"] = new[] { input.TargetLanguage }
        }
        .AddOptional("voice_id", input.VoiceId)
        .AddOptional("lipsync_enabled", input.LipSyncEnabled)
        .AddOptional("subtitle_enabled", input.SubtitleEnabled)
        .AddOptional("video_name", input.VideoName)
        .AddOptional("scripts", scripts);
        var request = new RestRequest("creative/aigc/video/task/create/", Method.Post)
            .AddJsonBody(new { aigc_video_type = "DUBBING", dubbing_video_info = new[] { item } });
        var response = await Client.ExecuteWithErrorHandling<ApiEnvelope<CreativeCreatedTasksResponse>>(request);

        return response.Data;
    }

    private static void Validate(CreateDubbingRequest input)
    {
        if (!input.ConsentConfirmed)
            throw new PluginMisconfigurationException("Collect the required affirmative end-user acknowledgement for this selected video before its initial API call, then set 'Video consent confirmed' to true. Do not infer consent.");

        CreativeValidation.MediaUrl(input.VideoUrl);
        InputValidator.Required(input.TargetLanguage, "Target language");
        CreativeValidation.Name(input.VideoName);
    }
}
