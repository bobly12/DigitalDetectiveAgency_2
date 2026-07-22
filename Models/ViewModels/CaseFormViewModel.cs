// Models/ViewModels/CaseFormViewModel.cs
using DigitalDetectiveAgency.Models.Entities;

namespace DigitalDetectiveAgency.Models.ViewModels;

public class CaseFormViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public CaseDifficulty Difficulty { get; set; }
    public bool IsPublished { get; set; }
}