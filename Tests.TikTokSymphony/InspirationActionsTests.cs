using Apps.TikTokSymphony.Actions;
using Tests.TikTokSymphony.Base;

namespace Tests.TikTokSymphony;

[TestClass]
public class InspirationActionsTests : TestBase
{
    [TestMethod]
    public async Task SearchTemplates_ReturnsTemplates()
    {
        var result = await new InspirationActions(InvocationContext).SearchTemplates(new() { MaximumResults = 5 });
        Assert.IsNotNull(result.Templates);
        PrintJsonResult(result);
    }

    [TestMethod]
    public async Task SearchTopAds_ReturnsTopAds()
    {
        var result = await new InspirationActions(InvocationContext).SearchTopAds(new() { MaximumResults = 5 });
        Assert.IsNotNull(result.TopAds);
        PrintJsonResult(result);
    }
}
