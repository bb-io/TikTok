using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;
using Apps.TikTokSymphony.Handlers.Creative;
using Apps.TikTokSymphony.Handlers.Static;
using Apps.TikTokSymphony.Constants;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.TikTokSymphony.Models.Identifiers.Creative;

public class DubbedVideoIdentifier
{
    [Display("Dubbed video ID", Description = "Choose an accessible finished dubbed video, or enter/map its ID. This is not a task ID.")]
    [DataSource(typeof(DubbedVideoDataHandler))]
    public string VideoId { get; set; } = string.Empty;
}
