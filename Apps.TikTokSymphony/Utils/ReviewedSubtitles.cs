using System.Globalization;
using System.Text.RegularExpressions;
using Blackbird.Applications.Sdk.Common.Exceptions;

namespace Apps.TikTokSymphony.Utils;

public static class ReviewedSubtitles
{
    public static List<Dictionary<string, object>> Parse(string text)
    {
        var blocks = Regex.Split(text.TrimStart('\uFEFF').Trim(), @"\r?\n(?:[ \t]*\r?\n)+");
        var segments = new List<Dictionary<string, object>>();
        

        foreach (var block in blocks)
        {
            var lines = block.Trim().Replace("\r\n", "\n").Split('\n');
            var timingIndex = lines.Length > 0 && Regex.IsMatch(lines[0].Trim(), @"^\d+$") ? 1 : 0;

            if (lines.Length <= timingIndex + 1)
                throw new PluginMisconfigurationException("Provide an SRT file with timestamps and translated text for every segment.");

            var timing = Regex.Match(lines[timingIndex].Trim(), @"^(\d{2,}):([0-5]\d):([0-5]\d),(\d{3})\s*-->\s*(\d{2,}):([0-5]\d):([0-5]\d),(\d{3})$");

            if (!timing.Success)
                throw new PluginMisconfigurationException("Use SRT timestamps in HH:MM:SS,mmm --> HH:MM:SS,mmm format, without styling settings.");

            var start = Milliseconds(timing, 1);
            var end = Milliseconds(timing, 5);
            var content = string.Join("\n", lines.Skip(timingIndex + 1)).Trim();

            if (!double.IsFinite(start) || !double.IsFinite(end) || start < 0 || end <= start || string.IsNullOrWhiteSpace(content))
                throw new PluginMisconfigurationException("Reviewed subtitle segments must contain text and have positive timing intervals.");

            segments.Add(new Dictionary<string, object>
            {
                ["text"] = content,
                ["text_start_time"] = start,
                ["text_end_time"] = end
            });
        }

        if (segments.Count == 0)
            throw new PluginMisconfigurationException("Provide at least one reviewed subtitle segment.");

        return segments;
    }

    private static double Milliseconds(Match match, int offset) =>
        double.Parse(match.Groups[offset].Value, CultureInfo.InvariantCulture) * 3600000
        + int.Parse(match.Groups[offset + 1].Value, CultureInfo.InvariantCulture) * 60000
        + int.Parse(match.Groups[offset + 2].Value, CultureInfo.InvariantCulture) * 1000
        + int.Parse(match.Groups[offset + 3].Value, CultureInfo.InvariantCulture);
}
