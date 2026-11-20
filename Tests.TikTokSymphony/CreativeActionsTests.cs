using Apps.TikTokSymphony.Actions;
using Apps.TikTokSymphony.Models.Requests.Creative;
using Blackbird.Applications.Sdk.Common.Files;
using Tests.TikTokSymphony.Base;

namespace Tests.TikTokSymphony;

[TestClass]
public class CreativeActionsTests : TestBase
{
    public TestContext TestContext { get; set; } = default!;

    [TestMethod]
    public async Task SearchDigitalAvatars_ReturnsCurrentCatalogue()
    {
        var result = await new CreativeCatalogueActions(InvocationContext).SearchDigitalAvatars(new() { MaximumResults = 150 });
        Assert.IsTrue(result.Avatars.Count > 0, "The test connection needs Digital Avatar API permission and allowlisting.");
        Assert.AreEqual(result.Avatars.Count, result.Avatars.Select(x => x.AvatarId).Distinct().Count());
    }

    [TestMethod]
    public async Task SearchVoices_ReturnsCurrentCatalogue()
    {
        var result = await new CreativeCatalogueActions(InvocationContext).SearchVoices(new() { MaximumResults = 150 });
        Assert.IsTrue(result.Voices.Count > 0);
        Assert.AreEqual(result.Voices.Count, result.Voices.Select(x => x.VoiceId).Distinct().Count());
    }

    [TestMethod]
    public async Task TranslateAndDubVideo_ReturnsTaskIds()
    {
        RequireGenerationOptIn();
        var result = await new DubbingActions(InvocationContext, new CreativeFileManager()).TranslateAndDubVideo(new()
        {
            VideoUrl = ApprovedVideo(),
            TargetLanguage = "fr",
            ConsentConfirmed = true
        });
        Assert.IsTrue(result.TaskIds.Count > 0);
        PrintJsonResult(result);
    }

    [TestMethod]
    public async Task DubVideoWithReviewedSubtitles_ReturnsTaskIds()
    {
        RequireGenerationOptIn();
        var result = await new DubbingActions(InvocationContext, new CreativeFileManager()).DubVideoWithReviewedSubtitles(new()
        {
            VideoUrl = ApprovedVideo(),
            TargetLanguage = "fr",
            ConsentConfirmed = true,
            Subtitles = new FileReference { Name = "reviewed-fr.srt", ContentType = "application/x-subrip" }
        });
        Assert.IsTrue(result.TaskIds.Count > 0);
        PrintJsonResult(result);
    }

    [TestMethod]
    public async Task GenerateAvatarVideo_ReturnsTaskId()
    {
        RequireGenerationOptIn();
        var avatars = await new CreativeCatalogueActions(InvocationContext).SearchDigitalAvatars(new() { Identity = "aigc", MaximumResults = 1 });
        Assert.IsTrue(avatars.Avatars.Count > 0);
        var result = await new AvatarActions(InvocationContext).GenerateAvatarVideo(new()
        {
            AvatarId = avatars.Avatars[0].AvatarId,
            Script = "Welcome to this demonstration of a creative workflow.",
            RightsConfirmed = true
        });
        Assert.IsTrue(result.Tasks.Any(x => !string.IsNullOrWhiteSpace(x.TaskId)));
        PrintJsonResult(result);
    }

    [TestMethod]
    public async Task GetDubbingTask_ReturnsSucceededFixture()
    {
        var result = await new DubbingTaskActions(InvocationContext).GetDubbingTask(new() { TaskId = Fixture("SucceededDubbingTaskId") });
        Assert.AreEqual("SUCCESS", result.Status);
        Assert.IsFalse(string.IsNullOrWhiteSpace(result.VideoId));
    }

    [TestMethod]
    public async Task GetAvatarTask_ReturnsSucceededFixture()
    {
        var result = await new AvatarActions(InvocationContext).GetAvatarTask(new() { TaskId = Fixture("SucceededAvatarTaskId") });
        Assert.AreEqual("SUCCESS", result.Status);
        Assert.IsFalse(string.IsNullOrWhiteSpace(result.VideoId));
    }

    [TestMethod]
    public async Task SearchDubbedVideos_ReturnsVideoMetadata()
    {
        var result = await new DubbingTaskActions(InvocationContext).SearchDubbedVideos(new() { MaximumResults = 5 });
        Assert.IsTrue(result.Videos.Count > 0, "Generate a dubbed video in the test account first.");
        Assert.IsTrue(result.Videos.All(x => !string.IsNullOrWhiteSpace(x.VideoId)));
    }

    [TestMethod]
    public async Task GetDubbedVideo_ReturnsSelectedVideoAndSegments()
    {
        var actions = new DubbingTaskActions(InvocationContext);
        var videos = await actions.SearchDubbedVideos(new() { MaximumResults = 1 });
        Assert.IsTrue(videos.Videos.Count > 0);
        var result = await actions.GetDubbedVideo(new() { VideoId = videos.Videos[0].VideoId! });
        Assert.AreEqual(videos.Videos[0].VideoId, result.VideoId);
        Assert.IsNotNull(result.DubbingInfo);
    }

    [TestMethod]
    public async Task SearchAvatarVideos_ReturnsAccessibleVideos()
    {
        var result = await new AvatarActions(InvocationContext).SearchAvatarVideos(new() { MaximumResults = 150 });
        Assert.IsTrue(result.Videos.Count > 0, "Generate an avatar video in the test account first.");
        Assert.AreEqual(result.Videos.Count, result.Videos.Select(x => x.VideoId).Distinct().Count());
    }

    [TestMethod]
    public async Task DownloadDubbedVideo_ReturnsNativeFile()
    {
        var result = await new DubbingTaskActions(InvocationContext).DownloadDubbedVideo(new() { TaskId = Fixture("SucceededDubbingTaskId") });
        Assert.IsNotNull(result.File);
        Assert.IsTrue(result.File.Name.EndsWith(".mp4") || result.File.Name.EndsWith(".mov"));
    }

    [TestMethod]
    public async Task DownloadDubbedSubtitles_ReturnsSrtFile()
    {
        var result = await new DubbingTaskActions(InvocationContext).DownloadDubbedSubtitles(new() { TaskId = Fixture("SucceededDubbingTaskId") });
        Assert.IsTrue(result.File.Name.EndsWith(".srt"));
    }

    [TestMethod]
    public async Task DownloadAvatarVideo_ReturnsNativeFile()
    {
        var result = await new AvatarActions(InvocationContext).DownloadAvatarVideo(new() { TaskId = Fixture("SucceededAvatarTaskId") });
        Assert.IsNotNull(result.File);
        Assert.IsTrue(result.File.Name.EndsWith(".mp4") || result.File.Name.EndsWith(".mov"));
    }

    [TestMethod]
    public async Task DownloadAvatarSubtitles_ReturnsSrtFile()
    {
        var result = await new AvatarActions(InvocationContext).DownloadAvatarSubtitles(new() { TaskId = Fixture("SucceededAvatarTaskId") });
        Assert.IsTrue(result.File.Name.EndsWith(".srt"));
    }

    private string Fixture(string key)
    {
        var value = TestContext.Properties[key]?.ToString();
        Assert.IsFalse(string.IsNullOrWhiteSpace(value), $"Set {key} in an ignored creative.runsettings file for the dedicated test account.");
        return value!;
    }

    private string ApprovedVideo()
    {
        Assert.AreEqual("true", Fixture("ApprovedSourceVideoConsent"), "Collect the documented per-video end-user acknowledgement before enabling this fixture.");
        return Fixture("ApprovedSourceVideoUrl");
    }

    private void RequireGenerationOptIn() =>
        Assert.AreEqual("true", Fixture("RunCreativeGeneration"), "Generation can incur charges. Enable only for a dedicated approved and allowlisted test account.");
}
