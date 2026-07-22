// Models/ViewModels/SuspectViewModel.cs
namespace DigitalDetectiveAgency.Models.ViewModels;

public class SuspectViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Motive { get; set; } = string.Empty;
    public string Alibi { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;

    // Note: IsGuilty is intentionally NOT included here.
    // Never expose the answer to the player through the ViewModel/HTML.
}