using Apps.TikTokSymphony.Actions;
using Apps.TikTokSymphony.Handlers.Creative;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Tests.TikTokSymphony.Base;

namespace Tests.TikTokSymphony;

[TestClass]
public class CreativeHandlerTests : TestBase
{
    [TestMethod]
    public async Task AvatarDataHandler_FindsAccessibleAvatarById()
    {
        var catalogue = await new CreativeCatalogueActions(InvocationContext).SearchDigitalAvatars(new() { MaximumResults = 1 });
        Assert.IsTrue(catalogue.Avatars.Count > 0);
        var id = catalogue.Avatars[0].AvatarId;
        var options = await new AvatarDataHandler(InvocationContext).GetDataAsync(new DataSourceContext { SearchString = id }, default);
        Assert.IsTrue(options.Any(x => x.Value == id));
    }

    [TestMethod]
    public async Task VoiceDataHandler_FindsAccessibleVoiceById()
    {
        var catalogue = await new CreativeCatalogueActions(InvocationContext).SearchVoices(new() { MaximumResults = 1 });
        Assert.IsTrue(catalogue.Voices.Count > 0);
        var id = catalogue.Voices[0].VoiceId;
        var options = await new VoiceDataHandler(InvocationContext).GetDataAsync(new DataSourceContext { SearchString = id }, default);
        Assert.IsTrue(options.Any(x => x.Value == id));
    }

    [TestMethod]
    public async Task DubbedVideoDataHandler_FindsAccessibleVideoById()
    {
        var catalogue = await new DubbingTaskActions(InvocationContext).SearchDubbedVideos(new() { MaximumResults = 1 });
        Assert.IsTrue(catalogue.Videos.Count > 0);
        var id = catalogue.Videos[0].VideoId!;
        var options = await new DubbedVideoDataHandler(InvocationContext).GetDataAsync(new DataSourceContext { SearchString = id }, default);
        Assert.IsTrue(options.Any(x => x.Value == id));
    }
}
