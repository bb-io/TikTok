using Apps.TikTokSymphony.Models.Identifiers;
using Apps.TikTokSymphony.Polling;
using Blackbird.Applications.Sdk.Common.Polling;
using Tests.TikTokSymphony.Base;

namespace Tests.TikTokSymphony;

[TestClass]
public class PollingTests : TestBase
{
    [TestMethod]
    public async Task OnVideoTaskFinished_FirstPollDoesNotFire_ThenFiresOnce()
    {
        var task = await FindSucceededTask();
        var events = new TaskPollingList(InvocationContext);
        var input = new TaskIdentifier { TaskId = task.TaskId };
        var baseline = await events.OnVideoTaskFinished(new PollingEventRequest<CompletionMemory>(), input);
        Assert.IsFalse(baseline.FlyBird);
        var completed = await events.OnVideoTaskFinished(new PollingEventRequest<CompletionMemory>
        {
            Memory = baseline.Memory
        }, input);
        Assert.IsTrue(completed.FlyBird);
        Assert.IsNotNull(completed.Result);
        Assert.AreEqual("SUCCEEDED", completed.Result.Status);
        var repeated = await events.OnVideoTaskFinished(new PollingEventRequest<CompletionMemory>
        {
            Memory = completed.Memory
        }, input);
        Assert.IsFalse(repeated.FlyBird);
    }
}
