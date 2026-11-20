namespace Apps.TikTokSymphony.Polling;

public class CreativeCompletionMemory
{
    public string TaskId { get; set; } = string.Empty;

    public string ConnectionKey { get; set; } = string.Empty;

    public string Kind { get; set; } = string.Empty;

    public bool Triggered { get; set; }
}
