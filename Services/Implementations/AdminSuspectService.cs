// Services/Implementations/AdminSuspectService.cs
using DigitalDetectiveAgency.Models.Entities;
using DigitalDetectiveAgency.Models.ViewModels;
using DigitalDetectiveAgency.Repositories.Interfaces;
using DigitalDetectiveAgency.Services.Interfaces;

namespace DigitalDetectiveAgency.Services.Implementations;

public class AdminSuspectService : IAdminSuspectService
{
    private readonly ISuspectRepository _suspectRepository;

    public AdminSuspectService(ISuspectRepository suspectRepository)
    {
        _suspectRepository = suspectRepository;
    }

    public async Task<SuspectFormViewModel?> GetByIdAsync(int id)
    {
        var s = await _suspectRepository.GetByIdAsync(id);
        if (s == null) return null;

        return new SuspectFormViewModel
        {
            Id = s.Id, CaseId = s.CaseId, Name = s.Name, Description = s.Description,
            Motive = s.Motive, Alibi = s.Alibi, ImageUrl = s.ImageUrl, IsGuilty = s.IsGuilty
        };
    }

    public async Task CreateAsync(SuspectFormViewModel form)
    {
        await _suspectRepository.CreateAsync(new Suspect
        {
            CaseId = form.CaseId,
            Name = form.Name,
            Description = form.Description,
            Motive = form.Motive,
            Alibi = form.Alibi,
            ImageUrl = form.ImageUrl,
            IsGuilty = form.IsGuilty
        });
    }

    public async Task UpdateAsync(SuspectFormViewModel form)
    {
        var entity = await _suspectRepository.GetByIdAsync(form.Id);
        if (entity == null) return;

        entity.Name = form.Name;
        entity.Description = form.Description;
        entity.Motive = form.Motive;
        entity.Alibi = form.Alibi;
        entity.ImageUrl = form.ImageUrl;
        entity.IsGuilty = form.IsGuilty;

        await _suspectRepository.UpdateAsync(entity);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _suspectRepository.GetByIdAsync(id);
        if (entity != null) await _suspectRepository.DeleteAsync(entity);
    }
}