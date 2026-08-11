// Repositories/Interfaces/IAccusationRepository.cs
using DigitalDetectiveAgency.Models.Entities;

namespace DigitalDetectiveAgency.Repositories.Interfaces;

public interface IAccusationRepository
{
    Task<Accusation?> GetByCaseAndUserAsync(int caseId, string userId);
    Task<List<Accusation>> GetAllByCaseAndUserAsync(int caseId, string userId); // NEW - full attempt history
    Task<int> GetAttemptCountAsync(int caseId, string userId);                 // NEW - tries used so far
    Task<Accusation> AddAsync(Accusation accusation);
}