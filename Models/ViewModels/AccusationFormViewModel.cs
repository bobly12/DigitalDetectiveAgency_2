// Models/ViewModels/AccusationFormViewModel.cs
namespace DigitalDetectiveAgency.Models.ViewModels;

public class AccusationFormViewModel
{
    public int CaseId { get; set; }
    public string CaseTitle { get; set; } = string.Empty;
    public List<SuspectOptionViewModel> Suspects { get; set; } = new();

    // NEW - tries system
    public int MaxAttempts { get; set; }
    public int AttemptsUsed { get; set; }
    public int AttemptsRemaining => Math.Max(0, MaxAttempts - AttemptsUsed);
    public List<int> WrongSuspectIds { get; set; } = new(); // suspects already ruled out by a wrong try
}