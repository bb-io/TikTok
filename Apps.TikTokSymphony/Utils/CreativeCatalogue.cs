using Apps.TikTokSymphony.Models.Responses.Creative;

namespace Apps.TikTokSymphony.Utils;

public static class CreativeCatalogue
{
    public static bool Matches(string? search, params string?[] values) =>
        string.IsNullOrWhiteSpace(search) || values.Any(x => x?.Contains(search.Trim(), StringComparison.OrdinalIgnoreCase) == true);

    public static string AvatarLabel(AvatarResponse avatar) =>
        $"{avatar.Name} · {string.Join(", ", avatar.Tags.SelectMany(x => x.Tags))} ({avatar.AvatarId})";

    public static string VoiceLabel(VoiceResponse voice) =>
        $"{voice.Name} · {string.Join(", ", voice.Tags.Select(x => x.Name))} ({voice.VoiceId})";

    public static bool AvatarMatches(AvatarResponse avatar, string? search) =>
        Matches(search, avatar.Name, avatar.AvatarId, string.Join(" ", avatar.Tags.SelectMany(x => x.Tags)));

    public static bool VoiceMatches(VoiceResponse voice, string? search) =>
        Matches(search, voice.Name, voice.VoiceId, string.Join(" ", voice.Tags.Select(x => x.Name)));
}
