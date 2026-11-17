using Apps.TikTokSymphony.Connections;
using Apps.TikTokSymphony.Constants;
using Tests.TikTokSymphony.Base;

namespace Tests.TikTokSymphony;

[TestClass]
public class CreativeConnectionTests : TestBase
{
    [TestMethod]
    public async Task ValidateConnection_WithoutTtoId_UsesCreativeCatalogue()
    {
        var credentials = Creds.Where(x => x.KeyName != CredsNames.TtoId);
        var result = await new ConnectionValidator(InvocationContext).ValidateConnection(credentials, default);
        Assert.IsTrue(result.IsValid, result.Message);
    }
}
