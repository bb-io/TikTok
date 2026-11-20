using Blackbird.Applications.Sdk.Common;

namespace Apps.TikTokSymphony.Models.Requests;

public class FrameSelection : GenerationSelection
{
    [Display("Last frame URL")]
    public string? LastFrameUrl { get; set; }
}
