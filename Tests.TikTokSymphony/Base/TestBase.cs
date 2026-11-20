using Apps.TikTokSymphony.Actions;
using Apps.TikTokSymphony.Models.Requests;
using Apps.TikTokSymphony.Models.Responses;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Tests.TikTokSymphony.Base;

public class TestBase
{
    protected TestBase()
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        Creds = config.GetSection("ConnectionDefinition").GetChildren()
            .Select(x => new AuthenticationCredentialsProvider(x.Key, x.Value ?? string.Empty)).ToList();
        InvocationContext = new InvocationContext { AuthenticationCredentialsProviders = Creds };
    }

    protected IEnumerable<AuthenticationCredentialsProvider> Creds { get; }

    protected InvocationContext InvocationContext { get; }

    protected async Task<VideoTaskResponse> FindSucceededTask(bool withLastFrame = false)
    {
        var result = await new TaskActions(InvocationContext).SearchVideoTasks(new SearchTasksRequest
        {
            Status = "SUCCEEDED"
        });
        var task = result.Tasks.FirstOrDefault(x => !withLastFrame || !string.IsNullOrEmpty(x.Video?.LastFrameUrl));
        Assert.IsNotNull(task, "Create a succeeded task in the test account first. Last-frame tests need a task created with Return last frame enabled.");
        return task;
    }

    protected static void PrintJsonResult(object result) =>
        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
}
