// Repositories/Interfaces/IBoardRepository.cs
using DigitalDetectiveAgency.Models.Entities;

namespace DigitalDetectiveAgency.Repositories.Interfaces;

public interface IBoardRepository
{
    Task<List<ClueConnection>> GetConnectionsAsync(int caseId, string userId);
    Task<ClueConnection> AddConnectionAsync(ClueConnection connection);
    Task<bool> ConnectionExistsAsync(int caseId, string userId, string fromType, int fromId, string toType, int toId);
    Task<ClueConnection?> GetConnectionByIdAsync(int connectionId, string userId);
    Task DeleteConnectionAsync(ClueConnection connection);
    Task<List<CaseConnection>> GetAnswerKeyAsync(int caseId);

    // NEW
    Task<List<int>> GetEliminatedSuspectIdsAsync(int caseId, string userId);
    Task ToggleEliminationAsync(int caseId, int suspectId, string userId);
    Task LogAttemptAsync(ConnectionAttempt attempt);
    Task<List<ConnectionAttempt>> GetAttemptsAsync(int caseId, string userId);
    Task<int> GetWrongAttemptCountAsync(int caseId, string userId);
    
    Task ResetProgressAsync(int caseId, string userId);
    
}