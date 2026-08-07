// Models/DTOs/ConnectionRequestDto.cs
namespace DigitalDetectiveAgency.Models.DTOs;

public class ConnectionRequestDto
{
    public int CaseId { get; set; }
    public string FromType { get; set; } = string.Empty;
    public int FromId { get; set; }
    public string ToType { get; set; } = string.Empty;
    public int ToId { get; set; }
}

public class DeleteConnectionRequestDto
{
    public int ConnectionId { get; set; }
    public int CaseId { get; set; } // NEW — needed so the server can rebuild the progress snapshot after deleting
}

public class ToggleEliminationRequestDto
{
    public int CaseId { get; set; }
    public int SuspectId { get; set; }
}

public class ResetCaseRequestDto
{
    public int CaseId { get; set; }
}