using Newtonsoft.Json;

namespace Apps.TikTokSymphony.Models.Dtos;

public class OAuthTokenDto
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; } = string.Empty;
}
