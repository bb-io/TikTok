using Apps.TikTokSymphony.Models.Responses;
using Apps.TikTokSymphony.Models.Responses.Creative;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.TikTokSymphony.Utils;

public static class CreativeFiles
{
    public static FileResponse FromTask(CreativeTaskResponse task, bool subtitles)
    {
        if (task.Status != "SUCCESS")
            throw new PluginMisconfigurationException($"The creative task is {task.Status}. Download is available after SUCCESS. {task.ErrorMessage}");

        var url = subtitles ? task.SubtitleUrl : task.PreviewUrl;

        if (string.IsNullOrWhiteSpace(url))
            throw new PluginMisconfigurationException(subtitles
                ? "No subtitle URL is available for this task. Check the result and seven-day subtitle retention period."
                : "No video URL is available for this succeeded task.");

        CreativeValidation.MediaUrl(url);
        var id = string.Concat((task.VideoId ?? task.TaskId).Select(x => char.IsLetterOrDigit(x) || x is '-' or '_' ? x : '_'));
        var mov = !subtitles && new Uri(url).AbsolutePath.EndsWith(".mov", StringComparison.OrdinalIgnoreCase);
        var extension = subtitles ? "srt" : mov ? "mov" : "mp4";
        var contentType = subtitles ? "application/x-subrip" : mov ? "video/quicktime" : "video/mp4";

        return new()
        {
            File = new FileReference(new HttpRequestMessage(HttpMethod.Get, url), $"{id}.{extension}", contentType)
        };
    }
}
