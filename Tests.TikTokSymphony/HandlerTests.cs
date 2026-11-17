using Apps.TikTokSymphony.Handlers;
using Apps.TikTokSymphony.Models.Requests;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Tests.TikTokSymphony.Base;

namespace Tests.TikTokSymphony;

[TestClass]
public class HandlerTests : TestBase
{
    [TestMethod]
    public async Task TemplateDataHandler_ReturnsItems()
    {
        var result = await new TemplateDataHandler(InvocationContext, new()).GetDataAsync(new DataSourceContext(), CancellationToken.None);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task TopAdDataHandler_ReturnsItems()
    {
        var result = await new TopAdDataHandler(InvocationContext, new()).GetDataAsync(new DataSourceContext(), CancellationToken.None);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task TaskDataHandler_ReturnsItems()
    {
        var result = await new TaskDataHandler(InvocationContext).GetDataAsync(new DataSourceContext(), CancellationToken.None);
        Assert.IsNotNull(result);
    }
    [TestMethod]
    public async Task SucceededTaskDataHandler_SuggestsSucceededTasks()
    {
        var items = (await new SucceededTaskDataHandler(InvocationContext)
            .GetDataAsync(new DataSourceContext(), CancellationToken.None)).ToList();
        Assert.IsTrue(items.Count > 0, "Prepare a succeeded task in the test account.");
        var task = await new Apps.TikTokSymphony.Actions.TaskActions(InvocationContext)
            .GetVideoTask(new() { TaskId = items[0].Value });
        Assert.AreEqual("SUCCEEDED", task.Status);
    }

    [TestMethod]
    public async Task LastFrameTaskDataHandler_SuggestsTasksWithFrames()
    {
        var items = (await new LastFrameTaskDataHandler(InvocationContext)
            .GetDataAsync(new DataSourceContext(), CancellationToken.None)).ToList();
        Assert.IsTrue(items.Count > 0, "Prepare a succeeded paid-token task with a last frame.");
        var task = await new Apps.TikTokSymphony.Actions.TaskActions(InvocationContext)
            .GetVideoTask(new() { TaskId = items[0].Value });
        Assert.IsFalse(string.IsNullOrWhiteSpace(task.Video?.LastFrameUrl));
    }

    [TestMethod]
    public async Task DeletableTaskDataHandler_ReturnsIdentifiedChoices()
    {
        var items = (await new DeletableTaskDataHandler(InvocationContext)
            .GetDataAsync(new DataSourceContext(), CancellationToken.None)).ToList();
        Assert.IsNotNull(items);
        Assert.IsTrue(items.All(x => !string.IsNullOrWhiteSpace(x.Value) && x.DisplayName.Contains(x.Value)));
    }

    [TestMethod]
    public async Task SearchTaskDataHandler_RespectsSelectedStatus()
    {
        var handler = new SearchTaskDataHandler(InvocationContext, new TaskSearchContext { Status = "SUCCEEDED" });
        var items = (await handler.GetDataAsync(new DataSourceContext(), CancellationToken.None)).ToList();
        Assert.IsTrue(items.Count > 0, "Prepare a succeeded task in the test account.");
        var task = await new Apps.TikTokSymphony.Actions.TaskActions(InvocationContext)
            .GetVideoTask(new() { TaskId = items[0].Value });
        Assert.AreEqual("SUCCEEDED", task.Status);
    }
}
