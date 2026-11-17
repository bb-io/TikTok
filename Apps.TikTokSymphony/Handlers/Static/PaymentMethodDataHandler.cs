using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.TikTokSymphony.Handlers.Static;

public class PaymentMethodDataHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData() =>
    [
        new("BONUS_CREDITS", "Bonus credits"),
        new("PAID_TOKENS", "BytePlus paid tokens")
    ];
}
