// Models/Entities/ClueConnection.cs
namespace DigitalDetectiveAgency.Models.Entities;

public class ClueConnection
{
    public int Id { get; set; }

    public string ApplicationUserId { get; set; } = string.Empty;
    public ApplicationUser ApplicationUser { get; set; } = null!;

    public int CaseId { get; set; }
    public Case Case { get; set; } = null!;

    public string FromType { get; set; } = string.Empty; // "Evidence" | "Suspect" | "Witness"
    public int FromId { get; set; }
    public string ToType { get; set; } = string.Empty;
    public int ToId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}