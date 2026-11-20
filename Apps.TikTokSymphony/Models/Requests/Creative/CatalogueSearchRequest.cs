using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.TikTokSymphony.Models.Requests.Creative;

public class CatalogueSearchRequest
{
    [Display("Search text")]
    public string? SearchText { get; set; }

    [Display("Maximum results", Description = "Leave empty to search the complete catalogue, bounded to 50 pages. A failed or incomplete search is reported.")]
    public int? MaximumResults { get; set; }
}
