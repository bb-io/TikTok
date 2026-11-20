using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;
using Apps.TikTokSymphony.Handlers.Creative;
using Apps.TikTokSymphony.Handlers.Static;
using Apps.TikTokSymphony.Constants;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.TikTokSymphony.Models.Requests.Creative;

public class CreateAvatarRequest
{
    [Display("Avatar ID", Description = "Search the current avatar catalogue by name, tags or ID; direct and mapped IDs are supported.")]
    [DataSource(typeof(AvatarDataHandler))]
    public string AvatarId { get; set; } = string.Empty;

    [Display("Script", Description = "Plain text up to 2,000 characters. Avoid emojis and unsupported special characters. Language is supplied by your script.")]
    public string Script { get; set; } = string.Empty;

    [Display("Asset rights confirmed", Description = CreativeNotices.Upload)]
    public bool RightsConfirmed { get; set; }

    [Display("Voice ID", Description = "Optional. Leave empty for the avatar default voice. Search names and tags, or enter/map the ID.")]
    [DataSource(typeof(VoiceDataHandler))]
    public string? VoiceId { get; set; }

    [Display("Voice volume", Description = "0–10, up to two decimal places. Leave empty for the provider default.")]
    public double? Volume { get; set; }

    [Display("Voice speed")]
    [StaticDataSource(typeof(VoiceSpeedDataHandler))]
    public string? Speed { get; set; }

    [Display("Transparent background", Description = "Provider default false.")]
    public bool? TransparentBackground { get; set; }

    [Display("Video name", Description = "Optional, up to 50 characters.")]
    public string? VideoName { get; set; }
}
