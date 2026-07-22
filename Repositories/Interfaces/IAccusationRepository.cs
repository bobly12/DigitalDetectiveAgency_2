// Repositories/Interfaces/IAccusationRepository.cs
using DigitalDetectiveAgency.Models.Entities;

namespace DigitalDetectiveAgency.Repositories.Interfaces;

public interface IAccusationRepository
{
    Task<Accusation?> GetByCaseAndUserAsync(int caseId, string userId);
    Task<Accusation> AddAsync(Accusation accusation);
}