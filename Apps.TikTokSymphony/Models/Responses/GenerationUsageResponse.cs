using Blackbird.Applications.Sdk.Common;

namespace Apps.TikTokSymphony.Models.Responses;

public class GenerationUsageResponse
{
    [Display("Total tasks")]
    public int TotalTasks { get; set; }

    [Display("Succeeded tasks")]
    public int SucceededTasks { get; set; }

    [Display("Failed tasks")]
    public int FailedTasks { get; set; }

    [Display("Expired tasks")]
    public int ExpiredTasks { get; set; }

    [Display("Canceled tasks")]
    public int CanceledTasks { get; set; }

    [Display("Queued tasks")]
    public int QueuedTasks { get; set; }

    [Display("Running tasks")]
    public int RunningTasks { get; set; }

    [Display("Bonus credits used")]
    public decimal BonusCreditsUsed { get; set; }

    [Display("Paid tokens used")]
    public decimal PaidTokensUsed { get; set; }

    [Display("Generated duration in seconds")]
    public double GeneratedDurationSeconds { get; set; }
}
