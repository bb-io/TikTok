using Blackbird.Applications.Sdk.Common;

namespace Apps.TikTokSymphony.Models.Requests;

public class GenerationSelection
{
    [Display("Payment method")]
    public string? PaymentMethod { get; set; }

    [Display("Model")]
    public string? Model { get; set; }
}
