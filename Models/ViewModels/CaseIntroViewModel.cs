using DigitalDetectiveAgency.Models.Entities;
// Models/ViewModels/CaseIntroViewModel.cs
namespace DigitalDetectiveAgency.Models.ViewModels;

public class CaseIntroViewModel
{
    public int CaseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public List<string> StoryBeats { get; set; } = new();
    public CaseDifficulty Difficulty { get; set; }
    public int? TimeLimitSeconds { get; set; }
    public int MaxAttempts { get; set; }
}