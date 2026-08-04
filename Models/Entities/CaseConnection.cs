// Models/Entities/CaseConnection.cs
namespace DigitalDetectiveAgency.Models.Entities;

// The "answer key" - correct connections for a case, used later in Phase 8 scoring
public class CaseConnection
{
    public int Id { get; set; }

    public int CaseId { get; set; }
    public Case Case { get; set; } = null!;

    public string FromType { get; set; } = string.Empty;
    public int FromId { get; set; }
    public string ToType { get; set; } = string.Empty;
    public int ToId { get; set; }
    public string? Note { get; set; }
}