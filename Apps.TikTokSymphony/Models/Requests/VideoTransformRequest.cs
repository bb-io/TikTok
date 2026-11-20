using Apps.TikTokSymphony.Handlers;
using Apps.TikTokSymphony.Handlers.Static;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.TikTokSymphony.Models.Requests;

public class VideoTransformRequest
{
    [Display("Prompt", Description = "Describe the change or continuation. The model is fixed to Seedance 2.5.")]
    public string Prompt { get; set; } = string.Empty;

    [Display("Source video URL", Description = "Public HTTPS MP4 or MOV URL. Map a generated task's Video URL, refreshing it with Get video task first if needed. Editing requires 4–30 seconds; extension requires 2–30 seconds. Maximum 200 MB.")]
    public string VideoUrl { get; set; } = string.Empty;

    [Display("Payment method", Description = "Generation consumes credits or linked BytePlus paid tokens. Account eligibility still applies.")]
    [StaticDataSource(typeof(PaymentMethodDataHandler))]
    public virtual string PaymentMethod { get; set; } = string.Empty;

    [Display("Resolution", Description = "Select paid tokens first. Documented Seedance 2.5 catalogue; leave empty with bonus credits.")]
    [DataSource(typeof(TransformResolutionCatalogueHandler))]
    public string? Resolution { get; set; }

    [Display("Generate audio", Description = "Paid tokens only. Leave empty with bonus credits.")]
    public bool? GenerateAudio { get; set; }

    [Display("Return last frame", Description = "Paid tokens only. Leave empty with bonus credits.")]
    public bool? ReturnLastFrame { get; set; }
}
