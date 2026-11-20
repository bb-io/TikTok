using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Apps.TikTokSymphony.Handlers;
using Apps.TikTokSymphony.Handlers.Static;
using Apps.TikTokSymphony.Models.Responses;
using Apps.TikTokSymphony.Models.Identifiers;

namespace Apps.TikTokSymphony.Models.Responses;

public class BonusCreditsResponse
{
    [Display("Bonus credit balance")]
    [JsonProperty("balance")]
    public decimal Balance { get; set; }

    [Display("Credits expire at")]
    [JsonProperty("expire_time")]
    public DateTime? ExpiresAt { get; set; }
}
