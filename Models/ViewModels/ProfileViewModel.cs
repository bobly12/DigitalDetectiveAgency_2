using System;
using System.Linq;

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

    private static readonly (int Threshold, string Name)[] RankLadder =
    {
        (0, "Rookie Detective"),
        (3, "Detective"),
        (6, "Senior Detective"),
        (int.MaxValue, "Chief Investigator")
    };

    public string? NextRank
    {
        get
        {
            var next = RankLadder.FirstOrDefault(r => CasesSolved < r.Threshold);
            return next.Threshold == int.MaxValue ? null : next.Name;
        }
    }

    public int XpProgressPercent
    {
        get
        {
            var currentIndex = Array.FindLastIndex(RankLadder, r => CasesSolved >= r.Threshold);
            var current = RankLadder[currentIndex];
            if (current.Threshold == 6 && CasesSolved >= 6) return 100;

            var next = RankLadder[currentIndex + 1];
            var span = next.Threshold - current.Threshold;
            var progress = CasesSolved - current.Threshold;
            return (int)Math.Clamp(progress * 100.0 / span, 0, 100);
        }
    }
}