// Models/Entities/Suspect.cs
namespace DigitalDetectiveAgency.Models.Entities;

public class Suspect
{
    public int Id { get; set; }

    public int CaseId { get; set; }
    public Case Case { get; set; } = null!;

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Motive { get; set; } = string.Empty;
    public string Alibi { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;

    // Used in Phase 7 (Accusation) to check if the player picked correctly
    public bool IsGuilty { get; set; } = false;
}