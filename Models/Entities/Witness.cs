// Models/Entities/Witness.cs
namespace DigitalDetectiveAgency.Models.Entities;

public class Witness
{
    public int Id { get; set; }

    public int CaseId { get; set; }
    public Case Case { get; set; } = null!;

    public string Name { get; set; } = string.Empty;
    public string Statement { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
}