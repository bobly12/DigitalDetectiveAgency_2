// Models/ViewModels/BoardNodeViewModel.cs
namespace DigitalDetectiveAgency.Models.ViewModels;

public class BoardNodeViewModel
{
    public string Type { get; set; } = string.Empty; // "Evidence" | "Suspect" | "Witness"
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
}