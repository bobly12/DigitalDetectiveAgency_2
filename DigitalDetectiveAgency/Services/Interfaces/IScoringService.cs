// Services/Interfaces/IScoringService.cs
namespace DigitalDetectiveAgency.Services.Interfaces;

public interface IScoringService
{
    int CalculateScore(bool isCorrectSuspect, int connectionMatchPercent);
}