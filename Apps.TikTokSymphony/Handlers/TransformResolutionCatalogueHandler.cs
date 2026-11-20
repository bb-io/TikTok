using Apps.TikTokSymphony.Models.Requests;
using Apps.TikTokSymphony.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.TikTokSymphony.Handlers;

public class TransformResolutionCatalogueHandler(InvocationContext invocationContext, [ActionParameter] TransformSelection input)
    : BaseInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    public Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken) =>
        Task.FromResult(GenerationCatalogue.Search(
            GenerationCatalogue.Resolutions(input.PaymentMethod, "SEEDANCE_2_5"), context.SearchString));
}
