using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.TikTokSymphony.Utils;

public static class GenerationCatalogue
{
    public static IEnumerable<DataSourceItem> Models(string? paymentMethod, bool frameMode)
    {
        if (paymentMethod is not "BONUS_CREDITS" and not "PAID_TOKENS")
            return [];

        if (frameMode && paymentMethod == "BONUS_CREDITS")
            return [new("SEEDANCE_2_0", "Seedance 2.0 — bonus-credit text/frame generation")];

        return
        [
            new("SEEDANCE_2_0", "Seedance 2.0 — standard generation"),
            new("SEEDANCE_2_0_FAST", "Seedance 2.0 Fast — faster generation"),
            new("SEEDANCE_2_0_MINI", "Seedance 2.0 Mini — lower credit use"),
            new("SEEDANCE_2_5", paymentMethod == "BONUS_CREDITS"
                ? "Seedance 2.5 — bonus-credit allowlist required"
                : "Seedance 2.5 — account/model access required")
        ];
    }

    public static IEnumerable<DataSourceItem> Resolutions(string? paymentMethod, string? model)
    {
        if (paymentMethod != "PAID_TOKENS")
            return [];

        return (model ?? "SEEDANCE_2_0") switch
        {
            "SEEDANCE_2_0" =>
            [
                new("480P", "480p"),
                new("720P", "720p — provider default"),
                new("1080P", "1080p"),
                new("4K", "4K — Seedance 2.0")
            ],
            "SEEDANCE_2_5" =>
            [
                new("480P", "480p"),
                new("720P", "720p — provider default"),
                new("1080P", "1080p")
            ],
            "SEEDANCE_2_0_FAST" or "SEEDANCE_2_0_MINI" =>
            [
                new("480P", "480p"),
                new("720P", "720p — provider default")
            ],
            _ => []
        };
    }

    public static IEnumerable<DataSourceItem> AspectRatios(string? paymentMethod, bool allowAdaptive,
        bool adaptiveOnly = false)
    {
        if (paymentMethod != "PAID_TOKENS")
            return [];

        if (adaptiveOnly)
            return [new("ADAPTIVE", "Adaptive — required for the selected frame mode")];

        var choices = new List<DataSourceItem>
        {
            new("9:16", "9:16 — vertical"),
            new("16:9", "16:9 — widescreen"),
            new("1:1", "1:1 — square"),
            new("4:3", "4:3 — standard"),
            new("3:4", "3:4 — portrait"),
            new("21:9", "21:9 — ultrawide")
        };

        if (allowAdaptive)
            choices.Add(new("ADAPTIVE", "Adaptive — follow source content"));

        return choices;
    }

    public static IEnumerable<DataSourceItem> Search(IEnumerable<DataSourceItem> items, string? search) =>
        items.Where(x => string.IsNullOrWhiteSpace(search)
            || x.Value.Contains(search.Trim(), StringComparison.OrdinalIgnoreCase)
            || x.DisplayName.Contains(search.Trim(), StringComparison.OrdinalIgnoreCase));
}
