// Services/Interfaces/IAdminEvidenceService.cs
using DigitalDetectiveAgency.Models.ViewModels;

namespace DigitalDetectiveAgency.Services.Interfaces;

public interface IAdminEvidenceService
{
    Task<EvidenceFormViewModel?> GetByIdAsync(int id);
    Task CreateAsync(EvidenceFormViewModel form);
    Task UpdateAsync(EvidenceFormViewModel form);
    Task DeleteAsync(int id);
}