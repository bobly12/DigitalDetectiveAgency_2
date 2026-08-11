// Models/ViewModels/AccusationResultViewModel.cs
namespace DigitalDetectiveAgency.Models.ViewModels;

public class AccusationResultViewModel
{
    public int CaseId { get; set; }
    public string CaseTitle { get; set; } = string.Empty;
    public string AccusedSuspectName { get; set; } = string.Empty;
    public string AccusedSuspectImageUrl { get; set; } = string.Empty; // for the mugshot/stamp animation
    public int CaseStrengthPercent { get; set; } // board connection match %
    public bool WasCorrect { get; set; }
    public int Score { get; set; }                // final weighted score
    public string DetectiveSummary { get; set; } = string.Empty;

    // NEW - tries system
    public int MaxAttempts { get; set; }
    public int AttemptsUsed { get; set; }
    public int AttemptsRemaining => Math.Max(0, MaxAttempts - AttemptsUsed);
    public bool CaseClosed { get; set; } // true = no more tries left (won OR ran out); false = can retry
}