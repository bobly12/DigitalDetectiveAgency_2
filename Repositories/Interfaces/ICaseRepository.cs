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
    // ICaseRepository.cs — add:
// ICaseRepository.cs — replace:
// Task MarkCompletedAsync(PlayerCase playerCase);
// with:
    Task CompleteWithScoreAsync(PlayerCase playerCase, int score);    // ICaseRepository.cs — add:
    Task SaveScoreAsync(PlayerCase playerCase, int score);
    
    // ICaseRepository.cs — add:
    Task<List<Case>> GetAllAsync();
    Task<Case> CreateAsync(Case caseEntity);
    Task UpdateAsync(Case caseEntity);
    Task DeleteAsync(Case caseEntity);
    Task<List<string>> GetAllUserIdsAsync();
    Task<bool> UserHasPlayerCaseAsync(string userId, int caseId);
    Task CreatePlayerCaseAsync(string userId, int caseId);
    Task<List<int>> GetPublishedCaseIdsAsync();
    
    Task<Case?> GetCaseByIdAsync(int caseId);
}