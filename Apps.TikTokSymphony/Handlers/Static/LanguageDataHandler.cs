using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.TikTokSymphony.Handlers.Static;

public class LanguageDataHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData() =>
    [
        new("EN", "English"),
        new("ES", "Spanish"),
        new("ID", "Indonesian"),
        new("JA", "Japanese"),
        new("KO", "Korean"),
        new("PT", "Portuguese"),
        new("ZH", "Simplified Chinese")
    ];
}
