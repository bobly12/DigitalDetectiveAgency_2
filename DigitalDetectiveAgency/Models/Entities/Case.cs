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
}