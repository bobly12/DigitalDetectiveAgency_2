// Models/Entities/Accusation.cs
namespace DigitalDetectiveAgency.Models.Entities;

public class Accusation
{
    public int Id { get; set; }

    public string ApplicationUserId { get; set; } = string.Empty;
    public ApplicationUser ApplicationUser { get; set; } = null!;

    public int CaseId { get; set; }
    public Case Case { get; set; } = null!;

    public int AccusedSuspectId { get; set; }
    public Suspect AccusedSuspect { get; set; } = null!;

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
}