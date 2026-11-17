using Blackbird.Applications.Sdk.Common.Exceptions;

namespace Apps.TikTokSymphony.Utils;

public static class CreativeValidation
{
    public static void MediaUrl(string? value)
    {
        if (!Uri.TryCreate(value, UriKind.Absolute, out var uri)
            || uri.Scheme is not "https" and not "http" || !string.IsNullOrEmpty(uri.UserInfo))
            throw new PluginMisconfigurationException("Provide a publicly accessible HTTP or HTTPS media URL without embedded credentials.");
    }

    public static void Name(string? value)
    {
        if (value?.Length > 50)
            throw new PluginMisconfigurationException("Limit 'Video name' to 50 characters.");
    }
}
