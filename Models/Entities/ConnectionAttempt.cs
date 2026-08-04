// Models/Entities/ConnectionAttempt.cs
namespace DigitalDetectiveAgency.Models.Entities;

// Logs every connection attempt, correct or not.
// Used to unlock suspect files on ANY attempted link (not just correct ones),
// while ClueConnection now only ever holds connections proven correct.
public class ConnectionAttempt
{
    public int Id { get; set; }

    public string ApplicationUserId { get; set; } = string.Empty;
    public ApplicationUser ApplicationUser { get; set; } = null!;

    public int CaseId { get; set; }
    public Case Case { get; set; } = null!;

    public string FromType { get; set; } = string.Empty;
    public int FromId { get; set; }
    public string ToType { get; set; } = string.Empty;
    public int ToId { get; set; }

    public bool WasCorrect { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}