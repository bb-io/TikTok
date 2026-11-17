using Apps.TikTokSymphony.Actions;
using Tests.TikTokSymphony.Base;

namespace Tests.TikTokSymphony;

[TestClass]
public class UsageActionsTests : TestBase
{
    [TestMethod]
    public async Task GetGenerationUsage_ReturnsRetainedTaskSummary()
    {
        var result = await new UsageActions(InvocationContext).GetGenerationUsage(new());
        Assert.IsTrue(result.TotalTasks >= 0);
        Assert.AreEqual(result.TotalTasks, result.SucceededTasks + result.FailedTasks + result.ExpiredTasks
            + result.CanceledTasks + result.QueuedTasks + result.RunningTasks);
        Assert.IsTrue(result.BonusCreditsUsed >= 0);
        Assert.IsTrue(result.PaidTokensUsed >= 0);
        PrintJsonResult(result);
    }
}
