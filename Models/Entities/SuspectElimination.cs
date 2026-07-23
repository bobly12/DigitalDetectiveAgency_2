namespace DigitalDetectiveAgency.Models.Entities;

public class SuspectElimination
{
    public int Id { get; set; }

    public string ApplicationUserId { get; set; } = string.Empty;
    public ApplicationUser ApplicationUser { get; set; } = null!;

    public int CaseId { get; set; }
    public Case Case { get; set; } = null!;

    public int SuspectId { get; set; }
    public Suspect Suspect { get; set; } = null!;

    public DateTime EliminatedAt { get; set; } = DateTime.UtcNow;
}