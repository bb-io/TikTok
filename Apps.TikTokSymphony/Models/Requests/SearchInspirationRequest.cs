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

public class SearchInspirationRequest
{
    [Display("Search text")]
    public string? Search { get; set; }

    [Display("Industry")]
    [StaticDataSource(typeof(IndustryDataHandler))]
    public string? Industry { get; set; }

    [Display("Maximum results", Description = "Defaults to 100.")]
    public int? MaximumResults { get; set; }
}
