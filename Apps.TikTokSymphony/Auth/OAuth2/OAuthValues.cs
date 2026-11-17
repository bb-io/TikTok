using Blackbird.Applications.Sdk.Common.Exceptions;

namespace Apps.TikTokSymphony.Auth.OAuth2;

public static class OAuthValues
{
    public static string Required(Dictionary<string, string> values, string key, string label)
    {
        if (!values.TryGetValue(key, out var value) || string.IsNullOrWhiteSpace(value))
            throw new PluginMisconfigurationException($"Fill in '{label}' in the OAuth connection and start sign-in again.");

        return value.Trim();
    }
}
