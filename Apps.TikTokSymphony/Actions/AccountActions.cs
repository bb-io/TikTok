using Apps.TikTokSymphony.Models.Dtos;
using Apps.TikTokSymphony.Models.Responses;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.TikTokSymphony.Actions;

[ActionList("Account")]
public class AccountActions(InvocationContext invocationContext) : Invocable(invocationContext)
{
    [Action("Get credit balance", Description = "Get remaining bonus credits and the linked BytePlus balance in USD.")]
    public async Task<CreditBalanceResponse> GetCreditBalance()
    {
        var request = new RestRequest("symphony/credit/get/")
            .AddQueryParameter("tto_id", Client.TtoId);
        var response = await Client.ExecuteWithErrorHandling<ApiEnvelope<CreditBalanceResponse>>(request);

        return response.Data;
    }
}
