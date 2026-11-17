using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.TikTokSymphony.Handlers.Static;

public class AvatarIdentityDataHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData() =>
    [
        new("aigc", "AI-generated avatar"),
        new("real", "Real-person avatar")
    ];
}
