using Apps.TikTokSymphony.Polling;
using Blackbird.Applications.Sdk.Common.Polling;
using Tests.TikTokSymphony.Base;

namespace Tests.TikTokSymphony;

[TestClass]
public class CreativePollingTests : TestBase
{
    public TestContext TestContext { get; set; } = default!;

    [TestMethod]
    public async Task OnDubbingTaskFinished_BaselinesThenFiresOnce()
    {
        var events = new CreativePollingList(InvocationContext);
        var input = new Apps.TikTokSymphony.Models.Identifiers.Creative.DubbingTaskIdentifier { TaskId = Fixture("SucceededDubbingTaskId") };
        var first = await events.OnDubbingTaskFinished(new(), input);
        Assert.IsFalse(first.FlyBird);
        Assert.IsNull(first.Result);
        var finished = await events.OnDubbingTaskFinished(new() { Memory = first.Memory }, input);
        Assert.IsTrue(finished.FlyBird);
        Assert.IsNotNull(finished.Result);
        Assert.AreEqual("SUCCESS", finished.Result.Status);
        var repeated = await events.OnDubbingTaskFinished(new() { Memory = finished.Memory }, input);
        Assert.IsFalse(repeated.FlyBird);
        Assert.IsNull(repeated.Result);
    }

    [TestMethod]
    public async Task OnAvatarTaskFinished_BaselinesThenFiresOnce()
    {
        var events = new CreativePollingList(InvocationContext);
        var input = new Apps.TikTokSymphony.Models.Identifiers.Creative.AvatarTaskIdentifier { TaskId = Fixture("SucceededAvatarTaskId") };
        var first = await events.OnAvatarTaskFinished(new(), input);
        Assert.IsFalse(first.FlyBird);
        Assert.IsNull(first.Result);
        var finished = await events.OnAvatarTaskFinished(new() { Memory = first.Memory }, input);
        Assert.IsTrue(finished.FlyBird);
        Assert.IsNotNull(finished.Result);
        Assert.AreEqual("SUCCESS", finished.Result.Status);
        var repeated = await events.OnAvatarTaskFinished(new() { Memory = finished.Memory }, input);
        Assert.IsFalse(repeated.FlyBird);
        Assert.IsNull(repeated.Result);
    }

    private string Fixture(string key)
    {
        var value = TestContext.Properties[key]?.ToString();
        Assert.IsFalse(string.IsNullOrWhiteSpace(value), $"Provide {key} in an ignored creative.runsettings file for the dedicated account.");
        return value!;
    }
}
