using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.TikTokSymphony.Handlers.Static;

public class RegionDataHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData() =>
    [
        new("AR", "Argentina"),
        new("AU", "Australia"),
        new("BR", "Brazil"),
        new("CA", "Canada"),
        new("CO", "Colombia"),
        new("FR", "France"),
        new("DE", "Germany"),
        new("ID", "Indonesia"),
        new("IT", "Italy"),
        new("JP", "Japan"),
        new("MY", "Malaysia"),
        new("MX", "Mexico"),
        new("NL", "Netherlands"),
        new("PK", "Pakistan"),
        new("PH", "Philippines"),
        new("RO", "Romania"),
        new("SA", "Saudi Arabia"),
        new("SG", "Singapore"),
        new("ZA", "South Africa"),
        new("KR", "South Korea"),
        new("ES", "Spain"),
        new("SE", "Sweden"),
        new("TH", "Thailand"),
        new("TR", "Turkey"),
        new("AE", "United Arab Emirates"),
        new("GB", "United Kingdom"),
        new("US", "United States"),
        new("VN", "Vietnam")
    ];
}
