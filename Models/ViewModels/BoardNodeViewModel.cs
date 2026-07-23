namespace DigitalDetectiveAgency.Models.ViewModels;

public class BoardNodeViewModel
{
    public string Type { get; set; } = string.Empty;
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Motive { get; set; }    // Suspects only
    public string? Alibi { get; set; }     // Suspects only
}