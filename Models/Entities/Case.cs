namespace DigitalDetectiveAgency.Models.Entities;

public class Case
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public CaseDifficulty Difficulty { get; set; }
    public bool IsPublished { get; set; }

    public ICollection<PlayerCase> PlayerCases { get; set; } = new List<PlayerCase>();
    public ICollection<Evidence> EvidenceItems { get; set; } = new List<Evidence>(); // <--- ADD THIS
    public ICollection<Suspect> Suspects { get; set; } = new List<Suspect>();
    public ICollection<Witness> Witnesses { get; set; } = new List<Witness>();

    public ICollection<ClueConnection> ClueConnections { get; set; } = new List<ClueConnection>();
    public ICollection<CaseConnection> CaseConnections { get; set; } = new List<CaseConnection>();
    public ICollection<Accusation> Accusations { get; set; } = new List<Accusation>();
    public IEnumerable<SuspectElimination>? SuspectEliminations { get; set; }
    public string VictimName { get; set; } = string.Empty;
    public string VictimOccupation { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
}