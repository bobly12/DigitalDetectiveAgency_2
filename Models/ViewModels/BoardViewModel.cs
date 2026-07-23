namespace DigitalDetectiveAgency.Models.ViewModels;

public class BoardViewModel
{
    public int CaseId { get; set; }
    public string CaseTitle { get; set; } = string.Empty;
    public string CaseSummary { get; set; } = string.Empty;
    public string VictimName { get; set; } = string.Empty;
    public string VictimOccupation { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public int? Score { get; set; }

    public List<BoardNodeViewModel> Evidence { get; set; } = new();
    public List<BoardNodeViewModel> Suspects { get; set; } = new();
    public List<BoardNodeViewModel> Witnesses { get; set; } = new();

    public List<BoardConnectionViewModel> Connections { get; set; } = new();
    public List<int> EliminatedSuspectIds { get; set; } = new();
}