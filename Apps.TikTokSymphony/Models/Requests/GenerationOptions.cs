using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Apps.TikTokSymphony.Handlers;
using Apps.TikTokSymphony.Handlers.Static;
using Apps.TikTokSymphony.Models.Responses;
using Apps.TikTokSymphony.Models.Identifiers;

namespace Apps.TikTokSymphony.Models.Requests;

public class GenerationOptions
{
    [Display("Payment method", Description = "Choose bonus credits or the linked BytePlus account.")]
    [StaticDataSource(typeof(PaymentMethodDataHandler))]
    public string PaymentMethod { get; set; } = string.Empty;

    [Display("Model", Description = "Select Payment method first. Maintained documented catalogue; defaults to Seedance 2.0. Account eligibility is checked by TikTok.")]
    [DataSource(typeof(ModelCatalogueHandler))]
    public virtual string? Model { get; set; }

    [Display("Duration in seconds", Description = "Leave empty for the provider default.")]
    public int? Duration { get; set; }

    [Display("Resolution", Description = "Select paid tokens first. Maintained catalogue filtered by the selected model; direct or mapped values remain supported.")]
    [DataSource(typeof(ResolutionCatalogueHandler))]
    public string? Resolution { get; set; }

    [Display("Aspect ratio", Description = "Select paid tokens first. Maintained catalogue filtered by the selected model; direct or mapped values remain supported.")]
    [DataSource(typeof(AspectRatioCatalogueHandler))]
    public virtual string? AspectRatio { get; set; }

    [Display("Callback URL", Description = "Optional HTTPS destination for task status updates.")]
    public string? CallbackUrl { get; set; }
}
