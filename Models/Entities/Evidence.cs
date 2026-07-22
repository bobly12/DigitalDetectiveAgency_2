namespace DigitalDetectiveAgency.Models.Entities;

public class Evidence
{
    public int Id { get; set; }

    public int CaseId { get; set; }
    public Case Case { get; set; } = null!;

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
}