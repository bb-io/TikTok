using Apps.TikTokSymphony.Handlers.Static;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.TikTokSymphony.Models.Requests;

public class EditVideoRequest : VideoTransformRequest
{
    [Display("Payment method", Description = "Editing requires BytePlus paid tokens and Seedance 2.5 eligibility. This action creates a billable generation task.")]
    [StaticDataSource(typeof(EditingPaymentMethodDataHandler))]
    public override string PaymentMethod { get; set; } = string.Empty;
}
