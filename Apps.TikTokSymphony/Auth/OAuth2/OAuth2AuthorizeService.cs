using Apps.TikTokSymphony.Constants;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication.OAuth2;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Microsoft.AspNetCore.WebUtilities;

namespace Apps.TikTokSymphony.Auth.OAuth2;

public class OAuth2AuthorizeService(InvocationContext invocationContext)
    : BaseInvocable(invocationContext), IOAuth2AuthorizeService
{
    public string GetAuthorizationUrl(Dictionary<string, string> values)
    {
        var appId = OAuthValues.Required(values, CredsNames.AppId, "App ID");
        OAuthValues.Required(values, CredsNames.AppSecret, "App secret");
        OAuthValues.Required(values, "state", "Blackbird authorization state");
        var state = values["state"];
        var redirect = InvocationContext.UriInfo?.AuthorizationCodeRedirectUri;

        if (redirect is null || redirect.Scheme != Uri.UriSchemeHttps)
            throw new PluginMisconfigurationException("Blackbird did not provide an HTTPS OAuth callback. Start sign-in from the app's connection screen.");

        return QueryHelpers.AddQueryString("https://business-api.tiktok.com/portal/auth",
            new Dictionary<string, string?>
            {
                ["app_id"] = appId,
                ["redirect_uri"] = redirect.ToString(),
                ["state"] = state
            });
    }
}
