using Apps.TikTokSymphony.Api;
using Apps.TikTokSymphony.Constants;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Connections;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.TikTokSymphony.Connections;

public class ConnectionValidator(InvocationContext invocationContext)
    : BaseInvocable(invocationContext), IConnectionValidator
{
    public async ValueTask<ConnectionValidationResponse> ValidateConnection(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        CancellationToken cancellationToken)
    {
        try
        {
            var credentials = authenticationCredentialsProviders.ToArray();
            var creative = !credentials.Any(x => x.KeyName == CredsNames.TtoId && !string.IsNullOrWhiteSpace(x.Value));
            using var client = new TikTokSymphonyClient(credentials, creative);
            var request = new RestRequest(creative ? "creative/aigc/voice/get/" : "symphony/task/get/")
                .AddQueryParameter("page_size", 1);

            if (!creative)
                request.AddQueryParameter("tto_id", client.TtoId);
            await client.ExecuteWithErrorHandling(request);

            return new() { IsValid = true, Message = "Success" };
        }
        catch (Exception ex)
        {
            return new() { IsValid = false, Message = ex.Message };
        }
    }
}
