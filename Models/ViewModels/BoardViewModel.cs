namespace DigitalDetectiveAgency.Models.ViewModels;

public class BoardViewModel
{
    public int CaseId { get; set; }
    public string CaseTitle { get; set; } = string.Empty;
    public string CaseSummary { get; set; } = string.Empty;
    public string VictimName { get; set; } = string.Empty;
    public string VictimOccupation { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public int? Score { get; set; }

    public List<BoardNodeViewModel> Evidence { get; set; } = new();
    public List<BoardNodeViewModel> Suspects { get; set; } = new();
    public List<BoardNodeViewModel> Witnesses { get; set; } = new();

    public List<BoardConnectionViewModel> Connections { get; set; } = new();
    public List<int> EliminatedSuspectIds { get; set; } = new();
    
    // ==========================
    // Investigation Progress
    // ==========================

    /// <summary>
    /// Overall investigation confidence (0–100).
    /// Calculated dynamically by InvestigationProgressService.
    /// Never stored in the database.
    /// </summary>
    public int Confidence { get; set; }

    /// <summary>
    /// Indicates whether the player has gathered enough evidence
    /// to submit an accusation.
    /// </summary>
    public bool CanAccuse { get; set; }

    /// <summary>
    /// Number of correct clue connections made.
    /// </summary>
    public int CorrectConnections { get; set; }

    /// <summary>
    /// Total required clue connections in the case.
    /// </summary>
    public int TotalRequiredConnections { get; set; }

    /// <summary>
    /// Total clue connections created by the player.
    /// </summary>
    public int PlayerConnections { get; set; }

    /// <summary>
    /// Number of incorrect connection attempts made by the player.
    /// </summary>
    public int WrongAttempts { get; set; }

    /// <summary>
    /// Number of innocent suspects correctly eliminated.
    /// </summary>
    public int CorrectEliminatedSuspects { get; set; }

    /// <summary>
    /// Total innocent suspects in the case.
    /// </summary>
    public int TotalInnocentSuspects { get; set; }

    /// <summary>
    /// Same hint text as the live progress snapshot, for first page load.
    /// </summary>
    public string NextFocusHint { get; set; } = string.Empty;
    
    public int RemainingConfidence =>
        Math.Max(0, 75 - Confidence);
    
    public List<BoardTriedPairViewModel> TriedWrongPairs { get; set; } = new();
    public string Difficulty { get; set; } = string.Empty; // "Easy" | "Medium" | "Hard"
    public int? TimeLimitSeconds { get; set; } // null = no limit, e.g. 600 for Hard
    public DateTime OpenedAtUtc { get; set; } // NEW - anchors the timer so it doesn't reset on page reload
}