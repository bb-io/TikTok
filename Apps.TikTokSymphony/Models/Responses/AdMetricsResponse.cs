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

public class AdMetricsResponse
{
    [Display("Click-through rate")]
    [JsonProperty("ctr")]
    public double? ClickThroughRate { get; set; }

    [Display("View-through rate")]
    [JsonProperty("vtr")]
    public double? ViewThroughRate { get; set; }

    [Display("Engagement rate")]
    [JsonProperty("engagement_rate")]
    public double? EngagementRate { get; set; }

    [Display("Video views")]
    [JsonProperty("video_views")]
    public long? VideoViews { get; set; }
}
