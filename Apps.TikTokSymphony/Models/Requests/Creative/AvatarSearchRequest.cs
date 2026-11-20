using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;
using Apps.TikTokSymphony.Handlers.Creative;
using Apps.TikTokSymphony.Handlers.Static;
using Apps.TikTokSymphony.Constants;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.TikTokSymphony.Models.Requests.Creative;

public class AvatarSearchRequest : CatalogueSearchRequest
{
    [Display("Avatar type")]
    [StaticDataSource(typeof(AvatarIdentityDataHandler))]
    public string? Identity { get; set; }
}
