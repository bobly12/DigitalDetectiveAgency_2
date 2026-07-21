// Services/Interfaces/IAdminCaseService.cs
using DigitalDetectiveAgency.Models.ViewModels;

namespace DigitalDetectiveAgency.Services.Interfaces;

public interface IAdminCaseService
{
    Task<List<CaseFormViewModel>> GetAllAsync();
    Task<CaseFormViewModel?> GetByIdAsync(int id);
    Task CreateAsync(CaseFormViewModel form);
    Task UpdateAsync(CaseFormViewModel form);
    Task DeleteAsync(int id);
    Task PublishAsync(int id);
}