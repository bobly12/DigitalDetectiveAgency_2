// Models/ViewModels/BoardViewModel.cs
namespace DigitalDetectiveAgency.Models.ViewModels;

public class BoardViewModel
{
    public int CaseId { get; set; }
    public string CaseTitle { get; set; } = string.Empty;

    public List<BoardNodeViewModel> Evidence { get; set; } = new();
    public List<BoardNodeViewModel> Suspects { get; set; } = new();
    public List<BoardNodeViewModel> Witnesses { get; set; } = new();

    public List<BoardConnectionViewModel> Connections { get; set; } = new();
}