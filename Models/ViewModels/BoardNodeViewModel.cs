namespace DigitalDetectiveAgency.Models.ViewModels;

public class BoardNodeViewModel
{
    public string Type { get; set; } = string.Empty;
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Suspects only
    public string? Motive { get; set; }
    public string? Alibi { get; set; }

    // Investigation Progress (Suspects only)

    /// <summary>
    /// True when the player has unlocked this suspect's motive.
    /// </summary>
    public bool IsMotiveUnlocked { get; set; }

    /// <summary>
    /// True when the player has unlocked this suspect's alibi.
    /// </summary>
    public bool IsAlibiUnlocked { get; set; }
}