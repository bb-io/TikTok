using Apps.TikTokSymphony.Models.Requests;
using Apps.TikTokSymphony.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.TikTokSymphony.Handlers;

public class ResolutionCatalogueHandler(InvocationContext invocationContext, [ActionParameter] GenerationSelection input)
    : BaseInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    public Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken) =>
        Task.FromResult(GenerationCatalogue.Search(
            GenerationCatalogue.Resolutions(input.PaymentMethod, input.Model), context.SearchString));
}
