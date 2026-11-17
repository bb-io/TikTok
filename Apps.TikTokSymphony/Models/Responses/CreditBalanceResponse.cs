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

public class CreditBalanceResponse
{
    [Display("Bonus credits")]
    [JsonProperty("bonus_credits")]
    public BonusCreditsResponse? BonusCredits { get; set; }

    [Display("BytePlus balance")]
    [JsonProperty("paid_tokens")]
    public PaidBalanceResponse? PaidTokens { get; set; }

    [Display("Top-up URL")]
    [JsonProperty("top_up_url")]
    public string? TopUpUrl { get; set; }
}
