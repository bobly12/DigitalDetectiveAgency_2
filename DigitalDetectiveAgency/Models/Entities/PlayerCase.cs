// Models/Entities/PlayerCase.cs
using System;

namespace DigitalDetectiveAgency.Models.Entities;

public class PlayerCase
{
    public int Id { get; set; }

    public string ApplicationUserId { get; set; } = string.Empty;
    public ApplicationUser ApplicationUser { get; set; } = null!;

    public int CaseId { get; set; }
    public Case Case { get; set; } = null!;

    public bool IsOpened { get; set; } = false;
    public bool IsCompleted { get; set; } = false;
    public int? Score { get; set; }

    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    public DateTime? OpenedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}