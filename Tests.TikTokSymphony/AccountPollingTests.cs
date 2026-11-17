using Apps.TikTokSymphony.Actions;
using Apps.TikTokSymphony.Polling;
using Blackbird.Applications.Sdk.Common.Polling;
using Tests.TikTokSymphony.Base;

namespace Tests.TikTokSymphony;

[TestClass]
public class AccountPollingTests : TestBase
{
    [TestMethod]
    public async Task OnGeneratedVideoReady_BaselinesAndRecognizesUnseenSucceededTask()
    {
        var source = await FindSucceededTask();
        var events = new AccountTaskPollingList(InvocationContext);
        var baseline = await events.OnGeneratedVideoReady(new());
        Assert.IsFalse(baseline.FlyBird);
        Assert.IsNotNull(baseline.Memory);
        baseline.Memory.SeenTaskIds.Remove(source.TaskId);
        var changed = await events.OnGeneratedVideoReady(new() { Memory = baseline.Memory });
        Assert.IsTrue(changed.FlyBird);
        Assert.IsNotNull(changed.Result);
        Assert.IsTrue(changed.Result.Any(x => x.TaskId == source.TaskId));
        Assert.IsTrue(changed.Result.All(x => x.Status == "SUCCEEDED"));
        Assert.IsTrue(changed.Result.Count <= 100);
        var repeated = await events.OnGeneratedVideoReady(new() { Memory = changed.Memory });
        Assert.IsNotNull(repeated.Result);
        Assert.IsFalse(repeated.Result.Any(x => x.TaskId == source.TaskId));
    }

    [TestMethod]
    public async Task OnVideoGenerationFailed_BaselinesAndRecognizesUnseenFailure()
    {
        var tasks = await new TaskActions(InvocationContext).SearchVideoTasks(new());
        var failure = tasks.Tasks.FirstOrDefault(x => x.Status is "FAILED" or "EXPIRED");
        Assert.IsNotNull(failure, "A recent failed or expired task is required in the dedicated test account.");
        var events = new AccountTaskPollingList(InvocationContext);
        var baseline = await events.OnVideoGenerationFailed(new());
        Assert.IsFalse(baseline.FlyBird);
        Assert.IsNotNull(baseline.Memory);
        baseline.Memory.SeenTaskIds.Remove(failure.TaskId);
        var changed = await events.OnVideoGenerationFailed(new() { Memory = baseline.Memory });
        Assert.IsTrue(changed.FlyBird);
        Assert.IsNotNull(changed.Result);
        Assert.IsTrue(changed.Result.Any(x => x.TaskId == failure.TaskId));
        Assert.IsTrue(changed.Result.All(x => x.Status is "FAILED" or "EXPIRED"));
        var repeated = await events.OnVideoGenerationFailed(new() { Memory = changed.Memory });
        Assert.IsNotNull(repeated.Result);
        Assert.IsFalse(repeated.Result.Any(x => x.TaskId == failure.TaskId));
    }
}
