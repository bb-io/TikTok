using Apps.TikTokSymphony.Auth.OAuth2;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication.OAuth2;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Metadata;

namespace Apps.TikTokSymphony;

public class Application(InvocationContext invocationContext)
    : BaseInvocable(invocationContext), IApplication, ICategoryProvider
{
    public string Name => "TikTok Symphony";

    public IEnumerable<ApplicationCategory> Categories
    {
        get => [ApplicationCategory.Marketing];
        set { }
    }

    public T GetInstance<T>()
    {
        if (typeof(T) == typeof(IOAuth2AuthorizeService))
            return (T)(object)new OAuth2AuthorizeService(InvocationContext);

        if (typeof(T) == typeof(IOAuth2TokenService))
            return (T)(object)new OAuth2TokenService(InvocationContext);

        throw new PluginMisconfigurationException("The requested authentication service is not supported.");
    }
}
