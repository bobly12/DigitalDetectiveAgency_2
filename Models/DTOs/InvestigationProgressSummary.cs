namespace DigitalDetectiveAgency.Models.DTOs;

/// <summary>
/// Internal service-layer data carrier returned by
/// IInvestigationProgressService.GetInvestigationProgressAsync().
///
/// This is NOT a ViewModel. BoardService maps these values onto the
/// existing BoardViewModel/BoardNodeViewModel properties that are
/// already defined there for presentation.
/// </summary>
public class InvestigationProgressSummary
{
    public int Confidence { get; set; }
    public bool CanAccuse { get; set; }
    public int CorrectConnections { get; set; }
    public int TotalRequiredConnections { get; set; }
    public int PlayerConnections { get; set; }
    public int CorrectEliminatedSuspects { get; set; }
    public int TotalInnocentSuspects { get; set; }
    public HashSet<int> UnlockedSuspectIds { get; set; } = new();

    /// <summary>
    /// Evidence IDs the player has "discovered" so far under staged reveal.
    /// Computed fresh every call - never persisted.
    /// </summary>
    public HashSet<int> UnlockedEvidenceIds { get; set; } = new();

    /// <summary>
    /// Witness IDs the player has "discovered" so far under staged reveal.
    /// Computed fresh every call - never persisted.
    /// </summary>
    public HashSet<int> UnlockedWitnessIds { get; set; } = new();
}