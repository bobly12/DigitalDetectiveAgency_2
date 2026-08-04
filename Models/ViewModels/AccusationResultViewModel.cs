// Models/ViewModels/AccusationResultViewModel.cs
namespace DigitalDetectiveAgency.Models.ViewModels;

public class AccusationResultViewModel
{
    public int CaseId { get; set; }
    public string CaseTitle { get; set; } = string.Empty;
    public string AccusedSuspectName { get; set; } = string.Empty;
    public int CaseStrengthPercent { get; set; } // board connection match %
    public bool WasCorrect { get; set; }          // NEW
    public int Score { get; set; }                // NEW - final weighted score
    public string DetectiveSummary { get; set; } = string.Empty; // NEW

}