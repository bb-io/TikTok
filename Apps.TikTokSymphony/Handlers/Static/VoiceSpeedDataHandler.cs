using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.TikTokSymphony.Handlers.Static;

public class VoiceSpeedDataHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData() =>
    [
        new("0.7x", "0.7×"),
        new("0.8x", "0.8×"),
        new("0.9x", "0.9×"),
        new("1.0x", "1.0×"),
        new("1.1x", "1.1×"),
        new("1.2x", "1.2×")
    ];
}
