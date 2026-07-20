using DigitalDetectiveAgency.Models.Entities;

namespace DigitalDetectiveAgency.Repositories.Interfaces;

public interface ICaseRepository
{
    Task<Case?> GetByIdAsync(int id);
    Task<List<PlayerCase>> GetAssignedCasesForUserAsync(string userId);
    Task<PlayerCase?> GetPlayerCaseAsync(int caseId, string userId);
    Task MarkOpenedAsync(PlayerCase playerCase);
    Task<List<Evidence>> GetEvidenceForCaseAsync(int caseId); // ADDED
    // ICaseRepository.cs — add:
    Task<List<Suspect>> GetSuspectsForCaseAsync(int caseId);
    Task<List<Witness>> GetWitnessesForCaseAsync(int caseId);
}