using Apps.TikTokSymphony.Api;
using Apps.TikTokSymphony.Constants;
using Apps.TikTokSymphony.Models.Dtos;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication.OAuth2;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.TikTokSymphony.Auth.OAuth2;

public class OAuth2TokenService(InvocationContext invocationContext)
    : BaseInvocable(invocationContext), IOAuth2TokenService
{
    public bool IsRefreshToken(Dictionary<string, string> values) => false;

    public async Task<Dictionary<string, string>> RequestToken(string state, string code,
        Dictionary<string, string> values, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        OAuthValues.Required(new Dictionary<string, string> { ["state"] = state }, "state", "Blackbird authorization state");
        var appId = OAuthValues.Required(values, CredsNames.AppId, "App ID");
        var appSecret = OAuthValues.Required(values, CredsNames.AppSecret, "App secret");
        var ttoId = values.GetValueOrDefault(CredsNames.TtoId)?.Trim();
        var authCode = !string.IsNullOrWhiteSpace(code)
            ? code
            : values.GetValueOrDefault("auth_code");

        if (string.IsNullOrWhiteSpace(authCode))
            throw new PluginMisconfigurationException("TikTok did not return an authorization code. Start sign-in again from Blackbird. The callback must forward code or auth_code.");

        using var client = new TikTokSymphonyClient(new[] { appSecret, authCode });
        var request = new RestRequest("oauth2/access_token/", Method.Post)
            .AddJsonBody(new
            {
                app_id = appId,
                secret = appSecret,
                auth_code = authCode,
                is_long_term = true,
                return_advertiser_ids = false
            });
        var response = await client.ExecuteWithErrorHandling<ApiEnvelope<OAuthTokenDto>>(request);
        var accessToken = response.Data?.AccessToken;

        if (string.IsNullOrWhiteSpace(accessToken))
            throw new PluginApplicationException("TikTok did not return an access token. Start a fresh authorization from Blackbird; do not reuse the one-time code.");

        var stored = new Dictionary<string, string>
        {
            [CredsNames.AccessToken] = accessToken,
            [CredsNames.AppId] = appId,
            [CredsNames.AppSecret] = appSecret,
            [CredsNames.OAuthManaged] = "true"
        };

        if (!string.IsNullOrWhiteSpace(ttoId))
            stored[CredsNames.TtoId] = ttoId;

        return stored;
    }

    public Task<Dictionary<string, string>> RefreshToken(Dictionary<string, string> values,
        CancellationToken cancellationToken) =>
        throw new PluginMisconfigurationException("This OAuth connection uses a long-term TikTok token and does not use refresh tokens. If access was revoked, reconnect through TikTok sign-in.");

    public async Task RevokeToken(Dictionary<string, string> values)
    {
        if (!values.TryGetValue(CredsNames.OAuthManaged, out var managed) || managed != "true")
            return;

        var appId = OAuthValues.Required(values, CredsNames.AppId, "App ID");
        var appSecret = OAuthValues.Required(values, CredsNames.AppSecret, "App secret");
        var accessToken = OAuthValues.Required(values, CredsNames.AccessToken, "Access token");
        using var client = new TikTokSymphonyClient(new[] { appSecret, accessToken });
        var request = new RestRequest("oauth2/revoke_token/", Method.Post)
            .AddHeader("Access-Token", accessToken)
            .AddJsonBody(new
            {
                app_id = appId,
                secret = appSecret,
                access_token = accessToken
            });
        await client.ExecuteWithErrorHandling(request);
    }
}
