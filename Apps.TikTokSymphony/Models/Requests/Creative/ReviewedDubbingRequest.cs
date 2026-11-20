using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.TikTokSymphony.Models.Requests.Creative;

public class ReviewedDubbingRequest : CreateDubbingRequest
{
    [Display("Reviewed subtitles", Description = "UTF-8 SRT containing the approved target-language text and original timing. Maximum 2 MiB. Its language must match Target language.")]
    public FileReference Subtitles { get; set; } = default!;
}
