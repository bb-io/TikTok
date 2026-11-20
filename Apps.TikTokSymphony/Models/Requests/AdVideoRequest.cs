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

public class AdVideoRequest : GenerationOptions
{
    [Display("Aspect ratio", Description = "Select paid tokens first. Choose a fixed aspect ratio for ad generation; Adaptive is not supported.")]
    [DataSource(typeof(AdAspectRatioCatalogueHandler))]
    public override string? AspectRatio { get; set; }

    [Display("Prompt")]
    public string Prompt { get; set; } = string.Empty;

    [Display("Product image URLs", Description = "Public HTTPS URLs.")]
    public List<string>? ImageUrls { get; set; }

    [Display("Template ID", Description = "Search accessible templates by name or enter/map the ID. Clear any other inspiration source before searching this catalogue.")]
    [DataSource(typeof(TemplateDataHandler))]
    public string? TemplateId { get; set; }

    [Display("Top ad ID", Description = "Search accessible top ads by name or enter/map the ID. Clear any other inspiration source before searching this catalogue.")]
    [DataSource(typeof(TopAdDataHandler))]
    public string? TopAdId { get; set; }

    [Display("Inspiration video URL", Description = "Use one inspiration source: a template, top ad, or public HTTPS video URL.")]
    public string? InspirationVideoUrl { get; set; }

    [Display("Language", Description = "Choose the output language explicitly. Applies to voiceover and on-screen text; no language is selected automatically.")]
    [StaticDataSource(typeof(LanguageDataHandler))]
    public string Language { get; set; } = string.Empty;

    [Display("Music")]
    [StaticDataSource(typeof(PreferenceDataHandler))]
    public string? Music { get; set; }

    [Display("Voiceover")]
    [StaticDataSource(typeof(PreferenceDataHandler))]
    public string? Voiceover { get; set; }

    [Display("Number of videos", Description = "1\u20133. Defaults to one. Each video consumes credits or tokens.")]
    public int? VideoCount { get; set; }
}
