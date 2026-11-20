using System.Net;
using Apps.TikTokSymphony.Constants;
using Apps.TikTokSymphony.Models.Dtos;
using Apps.TikTokSymphony.Models.Responses;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using Newtonsoft.Json;
using Polly;
using RestSharp;

namespace Apps.TikTokSymphony.Api;

public class TikTokSymphonyClient : BlackBirdRestClient
{
    private readonly string[] sensitiveValues;

    public TikTokSymphonyClient(IEnumerable<AuthenticationCredentialsProvider> credentials, bool creativeTools = false)
        : this(credentials.Select(x => x.Value), creativeTools)
    {
        var values = credentials.ToArray();
        var accessToken = RequiredCredential(values, CredsNames.AccessToken, "Access token");
        TtoId = creativeTools ? string.Empty : RequiredCredential(values, CredsNames.TtoId, "TikTok One account ID");
        this.AddDefaultHeader("Access-Token", accessToken);
    }

    public TikTokSymphonyClient(IEnumerable<string> secrets, bool creativeTools = false)
        : base(new RestClientOptions
        {
            BaseUrl = new Uri(creativeTools
                ? "https://business-api.tiktok.com/open_api/v1.3/"
                : "https://business-api.tiktok.com/open_api/v2.0/"),
            Timeout = TimeSpan.FromSeconds(90)
        })
    {
        sensitiveValues = secrets.Where(x => !string.IsNullOrEmpty(x)).Distinct().ToArray();
        TtoId = string.Empty;
    }

    public string TtoId { get; }

    public override async Task<RestResponse> ExecuteWithErrorHandling(RestRequest request)
    {
        RestResponse response;

        try
        {
            response = request.Method == Method.Get
                ? await Policy.HandleResult<RestResponse>(x => x.StatusCode == HttpStatusCode.TooManyRequests
                        && RetryDelay(1, x) <= TimeSpan.FromSeconds(60))
                    .WaitAndRetryAsync(3, (attempt, result, _) => RetryDelay(attempt, result.Result),
                        (_, _, _, _) => Task.CompletedTask)
                    .ExecuteAsync(() => ExecuteAsync(request))
                : await ExecuteAsync(request);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or IOException)
        {
            throw new PluginApplicationException(NetworkMessage(request));
        }

        if (!response.IsSuccessStatusCode || response.ResponseStatus != ResponseStatus.Completed)
            throw ConfigureErrorException(response);

        var envelope = Deserialize<ApiEnvelope<object>>(response.Content);

        if (envelope.Code != 0)
            throw ConfigureErrorException(response);

        return response;
    }

    public new async Task<T> ExecuteWithErrorHandling<T>(RestRequest request)
    {
        var response = await ExecuteWithErrorHandling(request);
        return Deserialize<T>(response.Content);
    }

    public async Task<List<T>> Paginate<T>(Func<int, int, RestRequest> requestFactory, int? maximumResults, Func<T, bool>? matches = null, int? maximumPages = null)
    {
        var items = new List<T>();
        var pageSize = matches is null ? Math.Min(maximumResults ?? 100, 100) : 100;

        for (var page = 1; ; page++)
        {
            var envelope = await ExecuteWithErrorHandling<ApiEnvelope<PageDto<T>>>(requestFactory(page, pageSize));
            var data = envelope.Data;
            var remaining = maximumResults.HasValue ? maximumResults.Value - items.Count : int.MaxValue;
            items.AddRange(data.Items.Where(x => matches?.Invoke(x) ?? true).Take(remaining));

            if ((maximumResults.HasValue && items.Count >= maximumResults.Value)
                || (data.PageInfo is not null && page >= data.PageInfo.TotalPage)
                || data.Items.Count < pageSize)
                break;

            if (maximumPages.HasValue && page >= maximumPages.Value)
                throw new PluginMisconfigurationException("The catalogue search is incomplete after its request limit. Narrow the search or set a smaller maximum result count.");
        }

        return items;
    }

    public async Task<IEnumerable<DataSourceItem>> SearchSuggestions<T>(
        Func<int, int, RestRequest> requestFactory,
        Func<T, string> value,
        Func<T, string> label,
        Func<T, bool> matches,
        string refineAdvice, int maximumPages = 10)
    {
        const int pageSize = 100;
        const int maximumSuggestions = 100;
        var suggestions = new Dictionary<string, DataSourceItem>(StringComparer.Ordinal);

        for (var page = 1; page <= maximumPages; page++)
        {
            var response = await ExecuteWithErrorHandling<ApiEnvelope<PageDto<T>>>(requestFactory(page, pageSize));
            var data = response.Data;

            foreach (var item in data.Items.Where(matches))
            {
                suggestions[value(item)] = new DataSourceItem(value(item), label(item));

                if (suggestions.Count > maximumSuggestions)
                    throw new PluginMisconfigurationException(
                        $"More than {maximumSuggestions} choices match. {refineAdvice} You can also enter or map an ID directly.");
            }

            if (data.PageInfo is not null ? page >= data.PageInfo.TotalPage : data.Items.Count < pageSize)
                return suggestions.Values.ToList();
        }

        throw new PluginMisconfigurationException(
            $"The dropdown search is incomplete after {maximumPages} pages. {refineAdvice} Use the corresponding Search action or enter/map an ID directly.");
    }

    public async Task<string> ReadTextFile(Func<Task<Stream>> open, int maximumBytes)
    {
        try
        {
            await using var stream = await open();
            using var buffer = new MemoryStream();
            var chunk = new byte[8192];
            int count;

            while ((count = await stream.ReadAsync(chunk)) > 0)
            {
                if (buffer.Length + count > maximumBytes)
                    throw new PluginMisconfigurationException("Limit 'Reviewed subtitles' to a UTF-8 SRT file of at most 2 MiB.");

                buffer.Write(chunk, 0, count);
            }

            return new System.Text.UTF8Encoding(false, true).GetString(buffer.GetBuffer(), 0, checked((int)buffer.Length));
        }
        catch (Exception ex) when (ex is not PluginMisconfigurationException && ex is not PluginApplicationException)
        {
            throw new PluginMisconfigurationException("The reviewed subtitle file could not be read. Provide an accessible UTF-8 SRT file of at most 2 MiB.");
        }
    }

    protected override Exception ConfigureErrorException(RestResponse response)
    {
        if (response.ResponseStatus != ResponseStatus.Completed)
            return new PluginApplicationException(response.Request?.Resource?.StartsWith("oauth2/", StringComparison.Ordinal) == true
                ? "TikTok did not confirm the authentication request. For sign-in, start a fresh OAuth authorization instead of reusing the one-time code."
                : "TikTok Symphony could not be reached. If this was a generation request, check recent video tasks before submitting it again to avoid duplicate charges.");

        ApiEnvelope<CreationErrorDto>? error = null;

        try
        {
            error = JsonConvert.DeserializeObject<ApiEnvelope<CreationErrorDto>>(response.Content ?? "");
        }
        catch (JsonException)
        {
        }

        var providerMessage = error?.Message ?? $"Request failed with HTTP status {(int)response.StatusCode}.";
        foreach (var secret in sensitiveValues)
            providerMessage = providerMessage.Replace(secret, "[redacted]", StringComparison.Ordinal);
        providerMessage = providerMessage[..Math.Min(providerMessage.Length, 300)];
        var message = $"TikTok Symphony: {providerMessage}";

        if (!string.IsNullOrEmpty(error?.RequestId))
            message += $" Request ID: {error.RequestId}.";

        if (error?.Code == 51010)
        {
            var ids = (error.Data?.Tasks.Select(x => x.TaskId) ?? [])
                .Concat(error.Data?.TaskIds ?? [])
                .Concat(error.Data?.Items.Select(x => x.TaskId ?? string.Empty) ?? [])
                .Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().Take(100);
            return new PluginApplicationException(
                $"{message} Do not retry automatically. Acceptance may be partial or unknown. " +
                $"Check any returned task IDs and finished video results before submitting again. Accepted task IDs: {string.Join(", ", ids)}.");
        }

        if (error?.Code == 40002 && response.Request?.Resource?.StartsWith("oauth2/", StringComparison.Ordinal) == true)
            return new PluginMisconfigurationException(
                $"{message} Check App ID and App secret. For sign-in, start a new authorization to obtain a fresh one-time code.");

        if (error?.Code is 40001 or 40104 or 40105
            || response.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
            return new PluginMisconfigurationException(
                $"{message} Check the connection credentials (including App ID and App secret for OAuth), TTO account ID for Seedance, or Generative AI Tool > Video Generation permission and TikTok allowlisting for dubbing/avatars. Reauthorize after adding permissions.");

        if (error?.Code is 40002 or 40007 or 51023
            || response.StatusCode is HttpStatusCode.BadRequest or HttpStatusCode.NotFound or HttpStatusCode.UnprocessableEntity)
            return new PluginMisconfigurationException(
                $"{message} Check the action inputs and access to the referenced assets.");

        if (response.StatusCode == HttpStatusCode.TooManyRequests || error?.Code == 40100)
            message += " The request limit was reached. Reduce concurrent requests and try later.";

        return new PluginApplicationException(message);
    }

    private static string RequiredCredential(AuthenticationCredentialsProvider[] credentials, string key, string label)
    {
        var value = credentials.FirstOrDefault(x => x.KeyName == key)?.Value;

        if (string.IsNullOrWhiteSpace(value))
            throw new PluginMisconfigurationException($"Fill in '{label}' in your connection.");

        return value.Trim();
    }

    private static T Deserialize<T>(string? content)
    {
        try
        {
            return JsonConvert.DeserializeObject<T>(content ?? "", new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            }) ?? throw new PluginApplicationException("TikTok Symphony returned an empty response. If this was a generation request, check recent video tasks before submitting again.");
        }
        catch (JsonException)
        {
            throw new PluginApplicationException("TikTok Symphony returned an unreadable response. If this was a generation request, check recent video tasks before submitting again.");
        }
    }

    private static TimeSpan RetryDelay(int attempt, RestResponse response)
    {
        var value = response.Headers?.FirstOrDefault(x =>
            string.Equals(x.Name, "Retry-After", StringComparison.OrdinalIgnoreCase))?.Value?.ToString();

        if (double.TryParse(value, System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out var seconds) && double.IsFinite(seconds) && seconds >= 0)
            return TimeSpan.FromSeconds(Math.Min(seconds, TimeSpan.MaxValue.TotalSeconds / 2));

        if (DateTimeOffset.TryParse(value, out var date))
            return TimeSpan.FromSeconds(Math.Max((date - DateTimeOffset.UtcNow).TotalSeconds, 0));

        return TimeSpan.FromSeconds(Math.Pow(2, attempt))
            + TimeSpan.FromMilliseconds(Random.Shared.Next(0, 500));
    }

    private static string NetworkMessage(RestRequest request)
    {
        if (request.Resource.StartsWith("oauth2/", StringComparison.Ordinal))
            return "TikTok did not confirm the authentication request. For sign-in, start a fresh OAuth authorization instead of reusing the one-time code.";

        return request.Method == Method.Get
            ? "TikTok Symphony could not be reached. Please try later."
            : "TikTok Symphony did not confirm the request. Check recent video tasks before submitting again to avoid duplicate charges.";
    }
}
