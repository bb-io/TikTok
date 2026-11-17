using Apps.TikTokSymphony.Auth.OAuth2;
using Apps.TikTokSymphony.Connections;
using Apps.TikTokSymphony.Constants;
using Tests.TikTokSymphony.Base;

namespace Tests.TikTokSymphony;

[TestClass]
public class OAuthLifecycleTests : TestBase
{
    public TestContext TestContext { get; set; } = null!;

    [TestMethod]
    public async Task RequestToken_ValidOneTimeCode_ValidatesAndRevokesDisposableToken()
    {
        Assert.IsTrue(string.Equals(TestContext.Properties["RunOAuthLifecycle"]?.ToString(), "true",
            StringComparison.OrdinalIgnoreCase),
            "Enable RunOAuthLifecycle in a local runsettings file only for a dedicated test TTO/developer app. This test revokes the authorization it creates.");
        var authCode = TestContext.Properties["OAuthAuthorizationCode"]?.ToString();
        Assert.IsFalse(string.IsNullOrWhiteSpace(authCode), "Supply a fresh one-time authorization code in local runsettings.");
        var values = Creds.ToDictionary(x => x.KeyName, x => x.Value);
        Assert.IsFalse(string.IsNullOrWhiteSpace(values.GetValueOrDefault(CredsNames.AppId)), "Configure app_id in the local ConnectionDefinition.");
        Assert.IsFalse(string.IsNullOrWhiteSpace(values.GetValueOrDefault(CredsNames.AppSecret)), "Configure app_secret in the local ConnectionDefinition.");
        var service = new OAuth2TokenService(InvocationContext);
        var stored = await service.RequestToken("integration-test-state", authCode!, values, CancellationToken.None);
        Assert.IsFalse(service.IsRefreshToken(stored));

        try
        {
            Assert.IsFalse(string.IsNullOrWhiteSpace(stored[CredsNames.AccessToken]));
            Assert.AreEqual(values[CredsNames.TtoId], stored[CredsNames.TtoId]);
            var providers = new ConnectionDefinition().CreateAuthorizationCredentialsProviders(stored);
            Assert.IsFalse(providers.Any(x => x.KeyName == CredsNames.AppSecret));
            var validation = await new ConnectionValidator(InvocationContext)
                .ValidateConnection(providers, CancellationToken.None);
            Assert.IsTrue(validation.IsValid, validation.Message);
        }
        finally
        {
            await service.RevokeToken(stored);
        }
    }
}
