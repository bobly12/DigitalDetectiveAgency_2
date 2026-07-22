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
}