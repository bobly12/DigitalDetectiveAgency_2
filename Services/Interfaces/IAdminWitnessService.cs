// Services/Interfaces/IAdminWitnessService.cs
using DigitalDetectiveAgency.Models.ViewModels;

namespace DigitalDetectiveAgency.Services.Interfaces;

public interface IAdminWitnessService
{
    Task<WitnessFormViewModel?> GetByIdAsync(int id);
    Task CreateAsync(WitnessFormViewModel form);
    Task UpdateAsync(WitnessFormViewModel form);
    Task DeleteAsync(int id);
}