using Apps.TikTokSymphony.Models.Requests;
using Apps.TikTokSymphony.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.TikTokSymphony.Handlers;

public class FrameAspectRatioCatalogueHandler(InvocationContext invocationContext, [ActionParameter] FrameSelection input)
    : BaseInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    public Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken) =>
        Task.FromResult(GenerationCatalogue.Search(
            GenerationCatalogue.AspectRatios(input.PaymentMethod, true,
                (input.Model ?? "SEEDANCE_2_0") == "SEEDANCE_2_5" || !string.IsNullOrWhiteSpace(input.LastFrameUrl)), context.SearchString));
}
