namespace DigitalDetectiveAgency.Models.DTOs;

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
}