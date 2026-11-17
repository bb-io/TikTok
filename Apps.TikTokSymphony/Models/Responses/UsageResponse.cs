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

public class UsageResponse
{
    [Display("Bonus credits used")]
    [JsonProperty("bonus_credits_used")]
    public decimal BonusCreditsUsed { get; set; }

    [Display("Paid tokens used")]
    [JsonProperty("paid_tokens_used")]
    public decimal PaidTokensUsed { get; set; }
}
