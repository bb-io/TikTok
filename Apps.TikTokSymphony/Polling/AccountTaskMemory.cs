namespace Apps.TikTokSymphony.Polling;

public class AccountTaskMemory
{
    public string TtoId { get; set; } = string.Empty;

    public string Outcome { get; set; } = string.Empty;

    public List<string> SeenTaskIds { get; set; } = [];
}
