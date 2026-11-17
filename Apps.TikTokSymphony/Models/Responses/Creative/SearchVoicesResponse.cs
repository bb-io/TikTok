using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.TikTokSymphony.Models.Responses.Creative;

public class SearchVoicesResponse
{
    [Display("Voices")]
    public List<VoiceResponse> Voices { get; set; } = [];
}
