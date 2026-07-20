// Models/ViewModels/WitnessViewModel.cs
namespace DigitalDetectiveAgency.Models.ViewModels;

public class WitnessViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Statement { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
}