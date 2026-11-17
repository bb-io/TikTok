using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.TikTokSymphony.Handlers.Static;

public class ObjectiveDataHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData() =>
    [
        new("TRAFFIC", "Traffic"),
        new("APP_INSTALL", "App install"),
        new("WEB_CONVERSION", "Web conversion"),
        new("VIDEO_VIEW", "Video view"),
        new("REACH", "Reach"),
        new("LEAD_GENERATION", "Lead generation"),
        new("PRODUCT_SALES", "Product sales"),
        new("APP_PROMOTION", "App promotion")
    ];
}
