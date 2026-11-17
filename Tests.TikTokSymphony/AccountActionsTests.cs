using Apps.TikTokSymphony.Actions;
using Tests.TikTokSymphony.Base;

namespace Tests.TikTokSymphony;

[TestClass]
public class AccountActionsTests : TestBase
{
    [TestMethod]
    public async Task GetCreditBalance_ReturnsBalances()
    {
        var result = await new AccountActions(InvocationContext).GetCreditBalance();
        Assert.IsNotNull(result);
        Assert.IsTrue(result.BonusCredits is not null || result.PaidTokens is not null);
        PrintJsonResult(result);
    }
}
