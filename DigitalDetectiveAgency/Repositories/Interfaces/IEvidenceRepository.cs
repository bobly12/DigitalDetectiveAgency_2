// Repositories/Interfaces/IEvidenceRepository.cs
using DigitalDetectiveAgency.Models.Entities;

namespace DigitalDetectiveAgency.Repositories.Interfaces;

public interface IEvidenceRepository
{
    Task<Evidence?> GetByIdAsync(int id);
    Task<Evidence> CreateAsync(Evidence evidence);
    Task UpdateAsync(Evidence evidence);
    Task DeleteAsync(Evidence evidence);
}