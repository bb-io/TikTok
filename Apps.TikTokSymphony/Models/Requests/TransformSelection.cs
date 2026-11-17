using Blackbird.Applications.Sdk.Common;

namespace Apps.TikTokSymphony.Models.Requests;

public class TransformSelection
{
    [Display("Payment method")]
    public string? PaymentMethod { get; set; }
}
