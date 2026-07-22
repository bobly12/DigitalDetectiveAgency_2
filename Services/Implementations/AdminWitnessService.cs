// Services/Implementations/AdminWitnessService.cs
using DigitalDetectiveAgency.Models.Entities;
using DigitalDetectiveAgency.Models.ViewModels;
using DigitalDetectiveAgency.Repositories.Interfaces;
using DigitalDetectiveAgency.Services.Interfaces;

namespace DigitalDetectiveAgency.Services.Implementations;

public class AdminWitnessService : IAdminWitnessService
{
    private readonly IWitnessRepository _witnessRepository;

    public AdminWitnessService(IWitnessRepository witnessRepository)
    {
        _witnessRepository = witnessRepository;
    }

    public async Task<WitnessFormViewModel?> GetByIdAsync(int id)
    {
        var w = await _witnessRepository.GetByIdAsync(id);
        if (w == null) return null;

        return new WitnessFormViewModel
        {
            Id = w.Id, CaseId = w.CaseId, Name = w.Name, Statement = w.Statement, ImageUrl = w.ImageUrl
        };
    }

    public async Task CreateAsync(WitnessFormViewModel form)
    {
        await _witnessRepository.CreateAsync(new Witness
        {
            CaseId = form.CaseId,
            Name = form.Name,
            Statement = form.Statement,
            ImageUrl = form.ImageUrl
        });
    }

    public async Task UpdateAsync(WitnessFormViewModel form)
    {
        var entity = await _witnessRepository.GetByIdAsync(form.Id);
        if (entity == null) return;

        entity.Name = form.Name;
        entity.Statement = form.Statement;
        entity.ImageUrl = form.ImageUrl;

        await _witnessRepository.UpdateAsync(entity);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _witnessRepository.GetByIdAsync(id);
        if (entity != null) await _witnessRepository.DeleteAsync(entity);
    }
}