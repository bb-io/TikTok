using Apps.TikTokSymphony.Actions;
using Apps.TikTokSymphony.Models.Requests;
using Tests.TikTokSymphony.Base;

namespace Tests.TikTokSymphony;

[TestClass]
public class GenerationActionsTests : TestBase
{
    private const string ReferenceImage = "https://business-api.tiktok.com/portal/images/favicon.png";

    [TestMethod]
    public async Task GenerateVideoFromText_ReturnsQueuedTask()
    {
        var result = await new GenerationActions(InvocationContext).GenerateVideoFromText(new TextVideoRequest
        {
            Prompt = "A small paper boat floating on a calm lake at sunrise.",
            PaymentMethod = "BONUS_CREDITS",
            Duration = 4
        });
        Assert.AreEqual(1, result.Tasks.Count);
        Assert.IsFalse(string.IsNullOrEmpty(result.Tasks[0].TaskId));
        PrintJsonResult(result);
    }

    [TestMethod]
    public async Task GenerateVideoFromImages_ReturnsQueuedTask()
    {
        var result = await new GenerationActions(InvocationContext).GenerateVideoFromImages(new ImageVideoRequest
        {
            Prompt = "Gently move this icon against a white background.",
            FirstFrameUrl = ReferenceImage,
            PaymentMethod = "BONUS_CREDITS",
            Duration = 4
        });
        Assert.AreEqual(1, result.Tasks.Count);
        Assert.IsFalse(string.IsNullOrEmpty(result.Tasks[0].TaskId));
        PrintJsonResult(result);
    }

    [TestMethod]
    public async Task GenerateVideoFromReferences_ReturnsQueuedTask()
    {
        var result = await new GenerationActions(InvocationContext).GenerateVideoFromReferences(new ReferenceVideoRequest
        {
            Prompt = "Show @Image1 on a clean white background with subtle camera motion.",
            ImageUrls = [ReferenceImage],
            PaymentMethod = "BONUS_CREDITS",
            Duration = 4
        });
        Assert.AreEqual(1, result.Tasks.Count);
        Assert.IsFalse(string.IsNullOrEmpty(result.Tasks[0].TaskId));
        PrintJsonResult(result);
    }

    [TestMethod]
    public async Task GenerateAdVideo_ReturnsQueuedTask()
    {
        var templates = await new InspirationActions(InvocationContext).SearchTemplates(new() { MaximumResults = 1 });
        Assert.IsTrue(templates.Templates.Count > 0, "The test account needs access to at least one template.");
        var result = await new GenerationActions(InvocationContext).GenerateAdVideo(new AdVideoRequest
        {
            Prompt = "Create a short video introducing a creative workflow tool.",
            PaymentMethod = "BONUS_CREDITS",
            Model = "SEEDANCE_2_0_MINI",
            TemplateId = templates.Templates[0].TemplateId,
            Language = "EN",
            VideoCount = 1
        });
        Assert.AreEqual(1, result.Tasks.Count);
        Assert.IsFalse(string.IsNullOrEmpty(result.Tasks[0].TaskId));
        PrintJsonResult(result);
    }
}
