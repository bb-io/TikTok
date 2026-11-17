using Blackbird.Applications.Sdk.Common;

namespace Apps.TikTokSymphony.Models.Requests;

public class InspirationSelection
{
    [Display("Template ID")]
    public string? TemplateId { get; set; }

    [Display("Top ad ID")]
    public string? TopAdId { get; set; }

    [Display("Inspiration video URL")]
    public string? InspirationVideoUrl { get; set; }
}
