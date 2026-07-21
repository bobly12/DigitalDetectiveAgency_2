// Services/Implementations/ScoringService.cs
using DigitalDetectiveAgency.Services.Interfaces;

namespace DigitalDetectiveAgency.Services.Implementations;

public class ScoringService : IScoringService
{
    private const double SuspectWeight = 0.70;
    private const double ConnectionWeight = 0.30;

    public int CalculateScore(bool isCorrectSuspect, int connectionMatchPercent)
    {
        double suspectPoints = isCorrectSuspect ? 100 : 0;
        double connectionPoints = connectionMatchPercent;

        double total = (suspectPoints * SuspectWeight) + (connectionPoints * ConnectionWeight);

        return (int)Math.Round(total);
    }
}