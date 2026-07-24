using DigitalDetectiveAgency.Models.DTOs;

namespace DigitalDetectiveAgency.Services.Interfaces;

public interface IInvestigationProgressService
{
    /// <summary>
    /// Calculates the player's current investigation progress.
    /// All values are derived from the current board state.
    /// Nothing is stored in the database.
    /// </summary>
    Task<InvestigationProgressSummary> GetInvestigationProgressAsync(
        int caseId,
        string userId);
}