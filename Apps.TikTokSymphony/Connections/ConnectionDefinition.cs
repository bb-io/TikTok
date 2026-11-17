using Apps.TikTokSymphony.Constants;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Connections;

namespace Apps.TikTokSymphony.Connections;

public class ConnectionDefinition : IConnectionDefinition
{
    public IEnumerable<ConnectionPropertyGroup> ConnectionPropertyGroups =>
    [
        new()
        {
            Name = "OAuth2",
            DisplayName = "Sign in with TikTok (OAuth)",
            AuthenticationType = ConnectionAuthenticationType.OAuth2,
            ConnectionProperties =
            [
                new(CredsNames.AppId)
                {
                    DisplayName = "App ID",
                    Description = "The App ID from your TikTok for Business developer app's Basic Information."
                },
                new(CredsNames.AppSecret)
                {
                    DisplayName = "App secret",
                    Description = "The Secret from your developer app. It is used only for server-side token exchange and revocation.",
                    Sensitive = true
                },
                new(CredsNames.TtoId)
                {
                    DisplayName = "TikTok One account ID",
                    Description = "Find this ID in the top-right account menu in Symphony Creative Studio. It is not the App ID or advertiser ID."
                }
            ]
        },
        new()
        {
            Name = "CreativeOAuth2",
            DisplayName = "Creative tools sign-in (dubbing and avatars)",
            AuthenticationType = ConnectionAuthenticationType.OAuth2,
            ConnectionProperties =
            [
                new(CredsNames.AppId)
                {
                    DisplayName = "App ID",
                    Description = "Developer app with Generative AI Tool > Video Generation permission and TikTok allowlisting."
                },
                new(CredsNames.AppSecret)
                {
                    DisplayName = "App secret",
                    Description = "Used for server-side token exchange and revocation.",
                    Sensitive = true
                }
            ]
        },
        new()
        {
            Name = "Creative tools token",
            DisplayName = "Creative tools access token (manual)",
            AuthenticationType = ConnectionAuthenticationType.Undefined,
            ConnectionProperties =
            [
                new(CredsNames.AccessToken)
                {
                    DisplayName = "Access token",
                    Description = "Token authorized for Generative AI Tool > Video Generation with TikTok allowlisting for dubbing and avatars.",
                    Sensitive = true
                }
            ]
        },
        new()
        {
            Name = "TikTok One account",
            DisplayName = "Access token (manual)",
            AuthenticationType = ConnectionAuthenticationType.Undefined,
            ConnectionProperties =
            [
                new(CredsNames.AccessToken)
                {
                    DisplayName = "Access token",
                    Description = "A TTO-authorized token with Symphony Seedance Video Generation permission.",
                    Sensitive = true
                },
                new(CredsNames.TtoId)
                {
                    DisplayName = "TikTok One account ID",
                    Description = "The TTO account authorized for this token. This is not an advertiser ID."
                }
            ]
        }
    ];

    public IEnumerable<AuthenticationCredentialsProvider> CreateAuthorizationCredentialsProviders(
        Dictionary<string, string> values) =>
        values.Where(x => x.Key is CredsNames.AccessToken or CredsNames.TtoId)
            .Select(x => new AuthenticationCredentialsProvider(x.Key, x.Value));
}
