using Blackbird.Applications.Sdk.Common.Exceptions;

namespace Apps.TikTokSymphony.Utils;

public static class InputValidator
{
    public static void Required(string? value, string label)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new PluginMisconfigurationException($"Please fill in '{label}'.");
    }

    public static void HttpsUrl(string? value, string label)
    {
        if (!Uri.TryCreate(value, UriKind.Absolute, out var uri)
            || uri.Scheme != Uri.UriSchemeHttps || !string.IsNullOrEmpty(uri.UserInfo))
            throw new PluginMisconfigurationException($"Provide a public HTTPS URL in '{label}'.");
    }

    public static void MaximumResults(int? value)
    {
        if (value <= 0)
            throw new PluginMisconfigurationException("Set 'Maximum results' to a positive number, or leave it empty.");
    }

    public static List<string> Urls(List<string>? values, string label)
    {
        var urls = values ?? [];

        foreach (var url in urls)
            HttpsUrl(url, label);

        return urls;
    }

    public static void TaskIds(List<string>? values)
    {
        if (values is null || values.Count == 0 || values.Any(string.IsNullOrWhiteSpace))
            throw new PluginMisconfigurationException("Provide at least one non-empty video task ID.");
    }
}
