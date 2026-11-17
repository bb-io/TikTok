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

[ActionList("Video generation")]
public class GenerationActions(InvocationContext invocationContext) : Invocable(invocationContext)
{
    [Action("Generate video from text", Description = "Start generating a video from a prompt. Outputs a task ID; generation consumes credits or paid tokens.")]
    public async Task<CreatedTasksResponse> GenerateVideoFromText([ActionParameter] TextVideoRequest input)
    {
        InputValidator.Required(input.Prompt, "Prompt");
        ValidateVideoOptions(input);
        RequireSeedance20ForBonusFrames(input);
        var content = new List<object> { new { type = "TEXT", text = input.Prompt } };
        var body = VideoBody(input, content);
        var request = new RestRequest("symphony/video/task/create/", Method.Post).AddJsonBody(body);
        var response = await Client.ExecuteWithErrorHandling<ApiEnvelope<CreatedTasksResponse>>(request);

        return response.Data;
    }

    [Action("Generate video from images", Description = "Animate a first frame and optionally a last frame. Outputs a task ID; generation consumes credits or paid tokens.")]
    public async Task<CreatedTasksResponse> GenerateVideoFromImages([ActionParameter] ImageVideoRequest input)
    {
        InputValidator.HttpsUrl(input.FirstFrameUrl, "First frame URL");
        ValidateVideoOptions(input);
        RequireSeedance20ForBonusFrames(input);
        var content = new List<object>();

        if (!string.IsNullOrWhiteSpace(input.Prompt))
            content.Add(new { type = "TEXT", text = input.Prompt });

        content.Add(new { type = "IMAGE", image = new { url = input.FirstFrameUrl }, role = "FIRST_FRAME" });

        if (!string.IsNullOrWhiteSpace(input.LastFrameUrl))
        {
            InputValidator.HttpsUrl(input.LastFrameUrl, "Last frame URL");
            content.Add(new { type = "IMAGE", image = new { url = input.LastFrameUrl }, role = "LAST_FRAME" });
        }

        var body = VideoBody(input, content);

        if (input.PaymentMethod == "PAID_TOKENS"
            && (Model(input) == "SEEDANCE_2_5" || !string.IsNullOrWhiteSpace(input.LastFrameUrl)))
        {
            if (input.AspectRatio is not null && input.AspectRatio != "ADAPTIVE")
                throw new PluginMisconfigurationException("Choose 'ADAPTIVE' for 'Aspect ratio' or leave it empty for this image-to-video mode.");

            body["ratio"] = "ADAPTIVE";
        }

        var request = new RestRequest("symphony/video/task/create/", Method.Post).AddJsonBody(body);
        var response = await Client.ExecuteWithErrorHandling<ApiEnvelope<CreatedTasksResponse>>(request);

        return response.Data;
    }

    [Action("Generate video from references", Description = "Generate a video using a prompt and image, video, or audio references. Outputs a task ID; generation consumes credits or paid tokens.")]
    public async Task<CreatedTasksResponse> GenerateVideoFromReferences([ActionParameter] ReferenceVideoRequest input)
    {
        InputValidator.Required(input.Prompt, "Prompt");
        ValidateVideoOptions(input);
        var images = InputValidator.Urls(input.ImageUrls, "Reference image URLs");
        var videos = InputValidator.Urls(input.VideoUrls, "Reference video URLs");
        var audio = InputValidator.Urls(input.AudioUrls, "Reference audio URLs");
        var is25 = Model(input) == "SEEDANCE_2_5";

        if (images.Count + videos.Count + audio.Count == 0)
            throw new PluginMisconfigurationException("Provide at least one reference image, video, or audio URL.");

        if (images.Count > (is25 ? 30 : 9) || videos.Count > (is25 ? 10 : 3)
            || audio.Count > (is25 ? 10 : 3) || images.Count + videos.Count + audio.Count > (is25 ? 50 : 12))
            throw new PluginMisconfigurationException("Reduce reference URLs to the limits for the selected model: Seedance 2.5 allows 30 images, 10 videos, and 10 audio clips; other models allow 9 images, 3 videos, and 3 audio clips, with 12 references in total.");

        if (!is25 && images.Count + videos.Count == 0)
            throw new PluginMisconfigurationException("Add a reference image or video, or use Seedance 2.5 for audio-only references.");

        var content = new List<object> { new { type = "TEXT", text = input.Prompt } };
        content.AddRange(images.Select(url => new { type = "IMAGE", image = new { url }, role = "REFERENCE_IMAGE" }));
        content.AddRange(videos.Select(url => new { type = "VIDEO", video = new { url }, role = "REFERENCE_VIDEO" }));
        content.AddRange(audio.Select(url => new { type = "AUDIO", audio = new { url }, role = "REFERENCE_AUDIO" }));
        var body = VideoBody(input, content);

        if (is25)
            body["omni_reference_task_type"] = "REFERENCE";

        var request = new RestRequest("symphony/video/task/create/", Method.Post).AddJsonBody(body);
        var response = await Client.ExecuteWithErrorHandling<ApiEnvelope<CreatedTasksResponse>>(request);

        return response.Data;
    }

    [Action("Generate ad video", Description = "Generate ad videos from a brief, product images, and optional inspiration. Outputs task IDs; each video consumes credits or paid tokens.")]
    public async Task<CreatedTasksResponse> GenerateAdVideo([ActionParameter] AdVideoRequest input)
    {
        InputValidator.Required(input.Prompt, "Prompt");
        InputValidator.Required(input.Language, "Language");
        ValidateCommonOptions(input);
        var images = InputValidator.Urls(input.ImageUrls, "Product image URLs");

        if (images.Count > (Model(input) == "SEEDANCE_2_5" ? 30 : 9))
            throw new PluginMisconfigurationException("Reduce product images to 9, or 30 when using Seedance 2.5.");

        if (input.VideoCount is < 1 or > 3)
            throw new PluginMisconfigurationException("Set 'Number of videos' between 1 and 3.");

        if (input.Duration.HasValue && input.Duration != 15
            && !(Model(input) == "SEEDANCE_2_5" && input.Duration == 30))
            throw new PluginMisconfigurationException("Set 'Duration in seconds' to 15, or 30 with Seedance 2.5.");

        if (input.AspectRatio == "ADAPTIVE")
            throw new PluginMisconfigurationException("Choose a fixed aspect ratio for ad video generation.");

        var sourceCount = new[] { input.TemplateId, input.TopAdId, input.InspirationVideoUrl }
            .Count(x => !string.IsNullOrWhiteSpace(x));

        if (sourceCount > 1)
            throw new PluginMisconfigurationException("Choose only one inspiration source: 'Template ID', 'Top ad ID', or 'Inspiration video URL'.");

        var content = new List<object> { new { type = "TEXT", text = input.Prompt } };
        content.AddRange(images.Select(url => new { type = "IMAGE", image = new { url }, role = "REFERENCE_IMAGE" }));

        if (!string.IsNullOrWhiteSpace(input.TemplateId))
            content.Add(new { type = "VIDEO", video = new { item_id = input.TemplateId, item_type = "TEMPLATE" }, role = "REFERENCE_VIDEO" });

        if (!string.IsNullOrWhiteSpace(input.TopAdId))
            content.Add(new { type = "VIDEO", video = new { item_id = input.TopAdId, item_type = "TOP_AD" }, role = "REFERENCE_VIDEO" });

        if (!string.IsNullOrWhiteSpace(input.InspirationVideoUrl))
        {
            InputValidator.HttpsUrl(input.InspirationVideoUrl, "Inspiration video URL");
            content.Add(new { type = "VIDEO", video = new { url = input.InspirationVideoUrl }, role = "REFERENCE_VIDEO" });
        }

        var body = CommonBody(input, content)
            .AddOptional("music", input.Music)
            .AddOptional("voiceover", input.Voiceover)
            .AddOptional("language", input.Language)
            .AddOptional("video_count", input.VideoCount);
        var request = new RestRequest("symphony/insight_to_video/task/create/", Method.Post).AddJsonBody(body);
        var response = await Client.ExecuteWithErrorHandling<ApiEnvelope<CreatedTasksResponse>>(request);

        return response.Data;
    }

    private Dictionary<string, object> CommonBody(GenerationOptions input, List<object> content) =>
        new Dictionary<string, object>
        {
            ["tto_id"] = Client.TtoId,
            ["payment_method"] = input.PaymentMethod,
            ["model"] = Model(input),
            ["content"] = content
        }
        .AddOptional("duration", input.Duration)
        .AddOptional("resolution", input.Resolution)
        .AddOptional("ratio", input.AspectRatio)
        .AddOptional("callback_url", input.CallbackUrl);

    private Dictionary<string, object> VideoBody(VideoGenerationOptions input, List<object> content) =>
        CommonBody(input, content)
            .AddOptional("generate_audio", input.GenerateAudio)
            .AddOptional("return_last_frame", input.ReturnLastFrame)
            .AddOptional("priority", input.Priority)
            .AddOptional("execution_expires_after", input.ExpiresAfterSeconds);

    private static string Model(GenerationOptions input) => input.Model ?? "SEEDANCE_2_0";

    private static void ValidateCommonOptions(GenerationOptions input)
    {
        InputValidator.Required(input.PaymentMethod, "Payment method");

        if (input.PaymentMethod is not "BONUS_CREDITS" and not "PAID_TOKENS")
            throw new PluginMisconfigurationException("Choose a supported payment method from the dropdown.");

        if (input.CallbackUrl is not null)
            InputValidator.HttpsUrl(input.CallbackUrl, "Callback URL");

        if (input.PaymentMethod == "BONUS_CREDITS" && (input.Resolution is not null || input.AspectRatio is not null))
            throw new PluginMisconfigurationException("Leave 'Resolution' and 'Aspect ratio' empty when using bonus credits.");

        if (input.Resolution == "4K" && Model(input) != "SEEDANCE_2_0")
            throw new PluginMisconfigurationException("4K resolution requires Seedance 2.0.");

        if (input.Resolution == "1080P" && Model(input) is "SEEDANCE_2_0_FAST" or "SEEDANCE_2_0_MINI")
            throw new PluginMisconfigurationException("1080P requires Seedance 2.0 or Seedance 2.5.");
    }

    private static void ValidateVideoOptions(VideoGenerationOptions input)
    {
        ValidateCommonOptions(input);
        var maximum = Model(input) == "SEEDANCE_2_5" ? 30 : 15;

        if (input.Duration.HasValue && !(input.PaymentMethod == "PAID_TOKENS" && input.Duration == -1)
            && (input.Duration < 4 || input.Duration > maximum))
            throw new PluginMisconfigurationException($"Set 'Duration in seconds' between 4 and {maximum}, or -1 with paid tokens to let AI decide.");

        if (input.ExpiresAfterSeconds is < 3600 or > 259200)
            throw new PluginMisconfigurationException("Set 'Expire after seconds' between 3,600 and 259,200.");

        if (input.Priority is < 0 or > 9)
            throw new PluginMisconfigurationException("Set 'Priority' between 0 and 9.");

        if (input.PaymentMethod == "BONUS_CREDITS"
            && (input.GenerateAudio.HasValue || input.ReturnLastFrame.HasValue || input.Priority.HasValue))
            throw new PluginMisconfigurationException("Leave 'Generate audio', 'Return last frame', and 'Priority' empty when using bonus credits.");
    }

    private static void RequireSeedance20ForBonusFrames(GenerationOptions input)
    {
        if (input.PaymentMethod == "BONUS_CREDITS" && Model(input) != "SEEDANCE_2_0")
            throw new PluginMisconfigurationException("Choose Seedance 2.0 for text-to-video or frame-based generation with bonus credits.");
    }
}
