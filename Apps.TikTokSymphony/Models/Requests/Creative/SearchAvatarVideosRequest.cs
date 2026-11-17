using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;
using Apps.TikTokSymphony.Handlers.Creative;
using Apps.TikTokSymphony.Handlers.Static;
using Apps.TikTokSymphony.Constants;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.TikTokSymphony.Models.Requests.Creative;

public class SearchAvatarVideosRequest : CatalogueSearchRequest
{
    [Display("Avatar ID")]
    [DataSource(typeof(AvatarDataHandler))]
    public string? AvatarId { get; set; }

    [Display("Created after", Description = "UTC date, inclusive.")]
    public DateTime? CreatedAfter { get; set; }

    [Display("Created before", Description = "UTC date, inclusive.")]
    public DateTime? CreatedBefore { get; set; }
}
