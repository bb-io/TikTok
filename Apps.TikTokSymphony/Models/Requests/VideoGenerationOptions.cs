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

public class VideoGenerationOptions : GenerationOptions
{
    [Display("Generate audio", Description = "Only available with paid tokens.")]
    public bool? GenerateAudio { get; set; }

    [Display("Return last frame", Description = "Only available with paid tokens.")]
    public bool? ReturnLastFrame { get; set; }

    [Display("Priority", Description = "0\u20139. Only available with paid tokens.")]
    public int? Priority { get; set; }

    [Display("Expire after seconds", Description = "3,600\u2013259,200; defaults to 48 hours.")]
    public int? ExpiresAfterSeconds { get; set; }
}
