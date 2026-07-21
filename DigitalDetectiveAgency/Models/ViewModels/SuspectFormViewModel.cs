// Models/ViewModels/SuspectFormViewModel.cs
namespace DigitalDetectiveAgency.Models.ViewModels;

public class SuspectFormViewModel
{
    public int Id { get; set; }
    public int CaseId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Motive { get; set; } = string.Empty;
    public string Alibi { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsGuilty { get; set; }
}