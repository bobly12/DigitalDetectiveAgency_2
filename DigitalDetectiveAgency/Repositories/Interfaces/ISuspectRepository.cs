// Repositories/Interfaces/ISuspectRepository.cs
using DigitalDetectiveAgency.Models.Entities;

namespace DigitalDetectiveAgency.Repositories.Interfaces;

public interface ISuspectRepository
{
    Task<Suspect?> GetByIdAsync(int id);
    Task<Suspect> CreateAsync(Suspect suspect);
    Task UpdateAsync(Suspect suspect);
    Task DeleteAsync(Suspect suspect);
}