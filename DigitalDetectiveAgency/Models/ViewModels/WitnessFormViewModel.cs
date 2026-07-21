// Models/ViewModels/WitnessFormViewModel.cs
namespace DigitalDetectiveAgency.Models.ViewModels;

public class WitnessFormViewModel
{
    public int Id { get; set; }
    public int CaseId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Statement { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
}