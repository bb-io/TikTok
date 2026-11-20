using Apps.TikTokSymphony.Connections;
using Blackbird.Applications.Sdk.Common.Authentication;
using Tests.TikTokSymphony.Base;

namespace Tests.TikTokSymphony;

[TestClass]
public class ConnectionValidatorTests : TestBase
{
    [TestMethod]
    public async Task ValidateConnection_ValidData_Succeeds()
    {
        var result = await new ConnectionValidator(InvocationContext).ValidateConnection(Creds, CancellationToken.None);
        Assert.IsTrue(result.IsValid, result.Message);
    }

    [TestMethod]
    public async Task ValidateConnection_InvalidToken_FailsWithoutThrowing()
    {
        var invalid = Creds.Select(x => new AuthenticationCredentialsProvider(x.KeyName,
            x.KeyName == "access_token" ? "invalid-test-token" : x.Value));
        var result = await new ConnectionValidator(InvocationContext).ValidateConnection(invalid, CancellationToken.None);
        Assert.IsFalse(result.IsValid);
        Assert.IsFalse(string.IsNullOrWhiteSpace(result.Message));
    }
}
