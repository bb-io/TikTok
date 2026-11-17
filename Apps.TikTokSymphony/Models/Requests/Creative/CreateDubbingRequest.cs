using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;
using Apps.TikTokSymphony.Handlers.Creative;
using Apps.TikTokSymphony.Handlers.Static;
using Apps.TikTokSymphony.Constants;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.TikTokSymphony.Models.Requests.Creative;

public class CreateDubbingRequest
{
    [Display("Source video URL", Description = CreativeNotices.Upload)]
    public string VideoUrl { get; set; } = string.Empty;

    [Display("Target language", Description = "Select explicitly. One language is generated per action; use a workflow loop for language variants.")]
    [StaticDataSource(typeof(DubbingLanguageDataHandler))]
    public string TargetLanguage { get; set; } = string.Empty;

    [Display("Video consent confirmed", Description = CreativeNotices.Dubbing)]
    public bool ConsentConfirmed { get; set; }

    [Display("Voice ID", Description = "Optional. Search voice names/tags or enter/map an ID. Leave empty to use the original speaker voice. Verify language suitability from voice tags.")]
    [DataSource(typeof(VoiceDataHandler))]
    public string? VoiceId { get; set; }

    [Display("Enable lip sync", Description = "Provider default false; may add 5–10 minutes and requires suitable visible speech.")]
    public bool? LipSyncEnabled { get; set; }

    [Display("Display subtitles", Description = "Provider default false. When enabled, TikTok renders subtitles on the video.")]
    public bool? SubtitleEnabled { get; set; }

    [Display("Video name", Description = "Optional, up to 50 characters.")]
    public string? VideoName { get; set; }
}
