using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.TikTokSymphony.Models.Responses.Creative;

public class ScriptSegmentResponse
{
    [Display("Text")]
    [JsonProperty("text")]
    public string Text { get; set; } = string.Empty;

    [Display("Start time in milliseconds")]
    [JsonProperty("text_start_time")]
    public double StartMilliseconds { get; set; }

    [Display("End time in milliseconds")]
    [JsonProperty("text_end_time")]
    public double EndMilliseconds { get; set; }
}
