using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Apps.TikTokSymphony.Handlers;
using Apps.TikTokSymphony.Handlers.Static;
using Apps.TikTokSymphony.Models.Responses;
using Apps.TikTokSymphony.Models.Identifiers;

namespace Apps.TikTokSymphony.Models.Responses;

public class TemplateResponse
{
    [Display("Template ID")]
    [JsonProperty("template_id")]
    public string TemplateId { get; set; } = string.Empty;

    [Display("Name")]
    [JsonProperty("template_name")]
    public string Name { get; set; } = string.Empty;

    [Display("Description")]
    [JsonProperty("description")]
    public string? Description { get; set; }

    [Display("Video URL")]
    [JsonProperty("video")]
    public string? VideoUrl { get; set; }

    [Display("Thumbnail URL")]
    [JsonProperty("thumbnail_url")]
    public string? ThumbnailUrl { get; set; }

    [Display("Use cases")]
    [JsonProperty("use_cases")]
    public List<string> UseCases { get; set; } = [];

    [Display("Updated at")]
    [JsonProperty("updated_time")]
    public DateTime? UpdatedAt { get; set; }
}
