using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Apps.TikTokSymphony.Handlers;
using Apps.TikTokSymphony.Handlers.Static;
using Apps.TikTokSymphony.Models.Responses;
using Apps.TikTokSymphony.Models.Identifiers;

namespace Apps.TikTokSymphony.Models.Dtos;

public class ApiEnvelope<T>
{
    [JsonProperty("code")]
    public int Code { get; set; }

    [JsonProperty("message")]
    public string? Message { get; set; }

    [JsonProperty("subcode")]
    public string? Subcode { get; set; }

    [JsonProperty("request_id")]
    public string? RequestId { get; set; }

    [JsonProperty("data")]
    public T Data { get; set; } = default!;
}
