using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.TikTokSymphony.Handlers.Static;

public class IndustryDataHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData() =>
    [
        new("Beauty & Personal Care", "Beauty & Personal Care"),
        new("Appliances", "Appliances"),
        new("Apps", "Apps"),
        new("Apparel & Accessories", "Apparel & Accessories"),
        new("Games", "Games"),
        new("Food & Beverage", "Food & Beverage"),
        new("Home Improvement", "Home Improvement"),
        new("Lead industry", "Lead industry"),
        new("Sports & Outdoor", "Sports & Outdoor"),
        new("Health", "Health")
    ];
}
