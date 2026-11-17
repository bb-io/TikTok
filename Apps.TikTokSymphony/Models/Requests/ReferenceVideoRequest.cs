using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Apps.TikTokSymphony.Handlers;
using Apps.TikTokSymphony.Handlers.Static;
using Apps.TikTokSymphony.Models.Responses;
using Apps.TikTokSymphony.Models.Identifiers;

namespace Apps.TikTokSymphony.Models.Requests;

public class ReferenceVideoRequest : VideoGenerationOptions
{
    [Display("Prompt")]
    public string Prompt { get; set; } = string.Empty;

    [Display("Reference image URLs", Description = "Public HTTPS URLs; up to 9 images, or 30 with Seedance 2.5.")]
    public List<string>? ImageUrls { get; set; }

    [Display("Reference video URLs", Description = "Public HTTPS URLs; up to 3 videos, or 10 with Seedance 2.5.")]
    public List<string>? VideoUrls { get; set; }

    [Display("Reference audio URLs", Description = "Public HTTPS URLs; up to 3 audio clips, or 10 with Seedance 2.5.")]
    public List<string>? AudioUrls { get; set; }
}
