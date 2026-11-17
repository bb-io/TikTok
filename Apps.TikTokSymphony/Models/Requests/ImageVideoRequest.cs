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

public class ImageVideoRequest : VideoGenerationOptions
{
    [Display("Aspect ratio", Description = "Select paid tokens first. Seedance 2.5 or a last frame requires Adaptive; incompatible selected values must be changed manually.")]
    [DataSource(typeof(FrameAspectRatioCatalogueHandler))]
    public override string? AspectRatio { get; set; }

    [Display("Model", Description = "Select Payment method first. Maintained catalogue for this generation mode. Bonus-credit text and frame modes use Seedance 2.0.")]
    [DataSource(typeof(FrameModelCatalogueHandler))]
    public override string? Model { get; set; }

    [Display("First frame URL", Description = "Public HTTPS image URL.")]
    public string FirstFrameUrl { get; set; } = string.Empty;

    [Display("Last frame URL", Description = "Optional public HTTPS image URL.")]
    public string? LastFrameUrl { get; set; }

    [Display("Prompt")]
    public string? Prompt { get; set; }
}
