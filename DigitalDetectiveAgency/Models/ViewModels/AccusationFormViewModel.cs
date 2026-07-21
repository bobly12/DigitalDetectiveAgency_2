// Models/ViewModels/AccusationFormViewModel.cs
namespace DigitalDetectiveAgency.Models.ViewModels;

public class AccusationFormViewModel
{
    public int CaseId { get; set; }
    public string CaseTitle { get; set; } = string.Empty;
    public List<SuspectOptionViewModel> Suspects { get; set; } = new();
}