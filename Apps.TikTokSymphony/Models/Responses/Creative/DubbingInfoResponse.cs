using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.TikTokSymphony.Models.Responses.Creative;

public class DubbingInfoResponse
{
    [Display("Original script segments", Description = "When reviewed scripts were supplied, TikTok returns those here too; this is not an independent source transcription.")]
    [JsonProperty("original_scripts")]
    public List<ScriptSegmentResponse> OriginalScripts { get; set; } = [];

    [Display("Translated script segments")]
    [JsonProperty("translated_scripts")]
    public List<ScriptSegmentResponse> TranslatedScripts { get; set; } = [];

    [Display("Target language")]
    [JsonProperty("target_language")]
    public string? TargetLanguage { get; set; }
}
