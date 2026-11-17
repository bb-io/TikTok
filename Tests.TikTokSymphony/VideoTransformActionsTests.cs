using Apps.TikTokSymphony.Actions;
using Apps.TikTokSymphony.Models.Responses;
using Tests.TikTokSymphony.Base;

namespace Tests.TikTokSymphony;

[TestClass]
public class VideoTransformActionsTests : TestBase
{
    [TestMethod]
    public async Task EditVideo_ReturnsGenerationTask()
    {
        var source = await FindSource();
        var result = await new VideoTransformActions(InvocationContext).EditVideo(new()
        {
            Prompt = "Change the lighting to a warm sunset while preserving the scene.",
            VideoUrl = source.Video!.Url,
            PaymentMethod = "PAID_TOKENS"
        });
        Assert.AreEqual(1, result.Tasks.Count);
        Assert.IsFalse(string.IsNullOrEmpty(result.Tasks[0].TaskId));
    }

    [TestMethod]
    public async Task ExtendVideo_ReturnsGenerationTask()
    {
        var source = await FindSource();
        var result = await new VideoTransformActions(InvocationContext).ExtendVideo(new()
        {
            Prompt = "Continue the camera movement through the scene.",
            VideoUrl = source.Video!.Url,
            PaymentMethod = "PAID_TOKENS",
            Duration = 4
        });
        Assert.AreEqual(1, result.Tasks.Count);
        Assert.IsFalse(string.IsNullOrEmpty(result.Tasks[0].TaskId));
    }

    private async Task<VideoTaskResponse> FindSource()
    {
        var tasks = await new TaskActions(InvocationContext).SearchVideoTasks(new() { Status = "SUCCEEDED" });
        var source = tasks.Tasks.FirstOrDefault(x => x.Video?.Duration is >= 4 and <= 30);
        Assert.IsNotNull(source, "Provide a recent succeeded 4–30 second video within the documented source-media limits, and an eligible paid-token test account.");
        return source;
    }
}
