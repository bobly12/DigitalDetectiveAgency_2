namespace DigitalDetectiveAgency.Models.ViewModels;

public class CaseListItemViewModel
{
    public int CaseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Difficulty { get; set; } = string.Empty;
    public bool IsOpened { get; set; }
    public bool IsCompleted { get; set; }
    public int? Score { get; set; }
}