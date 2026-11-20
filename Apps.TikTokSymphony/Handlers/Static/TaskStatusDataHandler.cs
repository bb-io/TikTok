using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.TikTokSymphony.Handlers.Static;

public class TaskStatusDataHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData() =>
    [
        new("QUEUED", "Queued"),
        new("RUNNING", "Running"),
        new("SUCCEEDED", "Succeeded"),
        new("FAILED", "Failed"),
        new("EXPIRED", "Expired"),
        new("CANCELLED", "Cancelled")
    ];
}
