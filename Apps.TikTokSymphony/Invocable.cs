using Apps.TikTokSymphony.Api;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.TikTokSymphony;

public class Invocable : BaseInvocable
{
    public Invocable(InvocationContext invocationContext, bool creativeTools = false) : base(invocationContext)
    {
        Client = new TikTokSymphonyClient(invocationContext.AuthenticationCredentialsProviders, creativeTools);
    }

    protected TikTokSymphonyClient Client { get; }
}
