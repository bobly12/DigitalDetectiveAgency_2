// Models/ViewModels/EvidenceFormViewModel.cs
namespace DigitalDetectiveAgency.Models.ViewModels;

public class EvidenceFormViewModel
{
    public int Id { get; set; }
    public int CaseId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
}