namespace DigitalDetectiveAgency.Models.ViewModels;

public class ProfileViewModel
{
    public string DetectiveName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int CasesSolved { get; set; }
    public int AverageScore { get; set; }

    public string Rank => CasesSolved switch
    {
        0 => "Rookie Detective",
        < 3 => "Detective",
        < 6 => "Senior Detective",
        _ => "Chief Investigator"
    };
}