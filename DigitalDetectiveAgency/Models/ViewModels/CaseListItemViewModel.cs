// Models/ViewModels/CaseListItemViewModel.cs
namespace DigitalDetectiveAgency.Models.ViewModels;

public class CaseListItemViewModel
{
    public int CaseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Difficulty { get; set; } = string.Empty;
    public bool IsOpened { get; set; }
    public bool IsCompleted { get; set; }
    public int? Score { get; set; }
    // CaseDetailViewModel.cs — add:
    public List<SuspectViewModel> Suspects { get; set; } = new();
    public List<WitnessViewModel> Witnesses { get; set; } = new();
}