namespace Apps.TikTokSymphony.Extensions;

public static class DictionaryExtensions
{
    public static Dictionary<string, object> AddOptional(this Dictionary<string, object> body,
        string key, object? value)
    {
        if (value is not null && (value is not string text || !string.IsNullOrWhiteSpace(text)))
            body[key] = value;

        return body;
    }
}
