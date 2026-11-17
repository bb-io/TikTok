using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.TikTokSymphony.Models.Responses.Creative;

public class SearchAvatarsResponse
{
    [Display("Avatars")]
    public List<AvatarResponse> Avatars { get; set; } = [];
}
