// Repositories/Interfaces/IWitnessRepository.cs
using DigitalDetectiveAgency.Models.Entities;

namespace DigitalDetectiveAgency.Repositories.Interfaces;

public interface IWitnessRepository
{
    Task<Witness?> GetByIdAsync(int id);
    Task<Witness> CreateAsync(Witness witness);
    Task UpdateAsync(Witness witness);
    Task DeleteAsync(Witness witness);
}