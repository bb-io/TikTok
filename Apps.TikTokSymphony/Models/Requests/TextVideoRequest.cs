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

public class TextVideoRequest : VideoGenerationOptions
{
    [Display("Model", Description = "Select Payment method first. Maintained catalogue for this generation mode. Bonus-credit text and frame modes use Seedance 2.0.")]
    [DataSource(typeof(FrameModelCatalogueHandler))]
    public override string? Model { get; set; }

    [Display("Prompt")]
    public string Prompt { get; set; } = string.Empty;
}
