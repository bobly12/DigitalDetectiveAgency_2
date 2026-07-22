// Services/Implementations/AdminEvidenceService.cs
using DigitalDetectiveAgency.Models.Entities;
using DigitalDetectiveAgency.Models.ViewModels;
using DigitalDetectiveAgency.Repositories.Interfaces;
using DigitalDetectiveAgency.Services.Interfaces;

namespace DigitalDetectiveAgency.Services.Implementations;

public class AdminEvidenceService : IAdminEvidenceService
{
    private readonly IEvidenceRepository _evidenceRepository;

    public AdminEvidenceService(IEvidenceRepository evidenceRepository)
    {
        _evidenceRepository = evidenceRepository;
    }

    public async Task<EvidenceFormViewModel?> GetByIdAsync(int id)
    {
        var e = await _evidenceRepository.GetByIdAsync(id);
        if (e == null) return null;

        return new EvidenceFormViewModel
        {
            Id = e.Id, CaseId = e.CaseId, Name = e.Name, Description = e.Description, ImageUrl = e.ImageUrl
        };
    }

    public async Task CreateAsync(EvidenceFormViewModel form)
    {
        await _evidenceRepository.CreateAsync(new Evidence
        {
            CaseId = form.CaseId,
            Name = form.Name,
            Description = form.Description,
            ImageUrl = form.ImageUrl
        });
    }

    public async Task UpdateAsync(EvidenceFormViewModel form)
    {
        var entity = await _evidenceRepository.GetByIdAsync(form.Id);
        if (entity == null) return;

        entity.Name = form.Name;
        entity.Description = form.Description;
        entity.ImageUrl = form.ImageUrl;

        await _evidenceRepository.UpdateAsync(entity);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _evidenceRepository.GetByIdAsync(id);
        if (entity != null) await _evidenceRepository.DeleteAsync(entity);
    }
}