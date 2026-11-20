using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.TikTokSymphony.Handlers.Static;

public class DubbingLanguageDataHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData() =>
    [
        new("ar", "Arabic"),
        new("en", "English"),
        new("fr", "French"),
        new("de", "German"),
        new("id", "Indonesian"),
        new("it", "Italian"),
        new("ja", "Japanese"),
        new("ko", "Korean"),
        new("pt", "Portuguese"),
        new("es", "Spanish"),
        new("th", "Thai"),
        new("tr", "Turkish"),
        new("vi", "Vietnamese")
    ];
}
