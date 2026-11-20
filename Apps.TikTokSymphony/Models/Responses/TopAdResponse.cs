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

public class TopAdResponse
{
    [Display("Top ad ID")]
    [JsonProperty("top_ad_id")]
    public string TopAdId { get; set; } = string.Empty;

    [Display("Name")]
    [JsonProperty("name")]
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

    [Display("Brand")]
    [JsonProperty("brand")]
    public string? Brand { get; set; }

    [Display("Industry")]
    [JsonProperty("industry_name")]
    public string? Industry { get; set; }

    [Display("Region")]
    [JsonProperty("region")]
    public string? Region { get; set; }

    [Display("Objective")]
    [JsonProperty("objective")]
    public string? Objective { get; set; }

    [Display("Performance")]
    [JsonProperty("metrics")]
    public AdMetricsResponse? Metrics { get; set; }
}
