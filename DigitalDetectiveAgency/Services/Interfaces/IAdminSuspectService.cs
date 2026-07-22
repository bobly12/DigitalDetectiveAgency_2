// Services/Interfaces/IAdminSuspectService.cs
using DigitalDetectiveAgency.Models.ViewModels;

namespace DigitalDetectiveAgency.Services.Interfaces;

public interface IAdminSuspectService
{
    Task<SuspectFormViewModel?> GetByIdAsync(int id);
    Task CreateAsync(SuspectFormViewModel form);
    Task UpdateAsync(SuspectFormViewModel form);
    Task DeleteAsync(int id);
}