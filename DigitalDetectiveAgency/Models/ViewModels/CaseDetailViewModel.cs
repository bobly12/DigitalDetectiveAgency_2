using System.Collections.Generic;

namespace DigitalDetectiveAgency.Models.ViewModels;

public class CaseDetailViewModel
{
    public int CaseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Difficulty { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public int? Score { get; set; }

    public List<EvidenceViewModel> Evidence { get; set; } = new(); // ADDED
    public List<WitnessViewModel> Witnesses { get; set; }
    public List<SuspectViewModel> Suspects { get; set; }
}