using Apps.TikTokSymphony.Actions;
using Apps.TikTokSymphony.Models.Identifiers;
using Apps.TikTokSymphony.Models.Requests;
using Tests.TikTokSymphony.Base;

namespace Tests.TikTokSymphony;

[TestClass]
public class TaskActionsTests : TestBase
{
    [TestMethod]
    public async Task SearchVideoTasks_ReturnsRecentTasks()
    {
        var result = await new TaskActions(InvocationContext).SearchVideoTasks(new() { MaximumResults = 150 });
        Assert.IsNotNull(result.Tasks);
        Assert.AreEqual(result.Tasks.Count, result.Tasks.Select(x => x.TaskId).Distinct().Count());
        PrintJsonResult(result);
    }

    [TestMethod]
    public async Task GetVideoTask_FirstSucceededTask_ReturnsMatchingId()
    {
        var task = await FindSucceededTask();
        var result = await new TaskActions(InvocationContext).GetVideoTask(new() { TaskId = task.TaskId });
        Assert.AreEqual(task.TaskId, result.TaskId);
        Assert.AreEqual("SUCCEEDED", result.Status);
    }

    [TestMethod]
    public async Task DownloadGeneratedVideo_ReturnsVideoFile()
    {
        var task = await FindSucceededTask();
        var result = await new TaskActions(InvocationContext).DownloadGeneratedVideo(new() { TaskId = task.TaskId });
        Assert.IsNotNull(result.File);
        Assert.IsTrue(result.File.Name.EndsWith(".mp4") || result.File.Name.EndsWith(".mov"));
    }

    [TestMethod]
    public async Task DownloadGeneratedVideos_ReturnsDistinctFilesInRequestedOrder()
    {
        var task = await FindSucceededTask();
        var result = await new TaskActions(InvocationContext).DownloadGeneratedVideos(new()
        {
            TaskIds = [task.TaskId, task.TaskId]
        });
        Assert.AreEqual(1, result.Videos.Count);
        Assert.AreEqual(task.TaskId, result.Videos[0].TaskId);
        Assert.IsNotNull(result.Videos[0].File);
    }

    [TestMethod]
    public async Task DownloadLastFrame_ReturnsJpegFile()
    {
        var task = await FindSucceededTask(true);
        var result = await new TaskActions(InvocationContext).DownloadLastFrame(new() { TaskId = task.TaskId });
        Assert.IsNotNull(result.File);
        Assert.IsTrue(result.File.Name.EndsWith(".jpg"));
    }

    [TestMethod]
    public async Task CancelOrDeleteVideoTasks_OnlyOwnNewTask_ReturnsCancellation()
    {
        var created = await new GenerationActions(InvocationContext).GenerateVideoFromText(new TextVideoRequest
        {
            Prompt = "A paper boat on calm water.",
            PaymentMethod = "BONUS_CREDITS",
            Duration = 4
        });
        Assert.AreEqual(1, created.Tasks.Count);
        var result = await new TaskActions(InvocationContext).CancelOrDeleteVideoTasks(new DeleteTasksRequest
        {
            TaskIds = [created.Tasks[0].TaskId]
        });
        Assert.AreEqual(1, result.Results.Count);
        Assert.AreEqual(created.Tasks[0].TaskId, result.Results[0].TaskId);
        Assert.IsTrue(result.Results[0].Result is "CANCELLED" or "DELETED", result.Results[0].FailureReason);
        PrintJsonResult(result);
    }
}
