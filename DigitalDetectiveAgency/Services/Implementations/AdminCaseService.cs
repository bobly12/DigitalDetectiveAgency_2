// Services/Implementations/AdminCaseService.cs
using DigitalDetectiveAgency.Models.Entities;
using DigitalDetectiveAgency.Models.ViewModels;
using DigitalDetectiveAgency.Repositories.Interfaces;
using DigitalDetectiveAgency.Services.Interfaces;

namespace DigitalDetectiveAgency.Services.Implementations;

public class AdminCaseService : IAdminCaseService
{
    private readonly ICaseRepository _caseRepository;
    private readonly ICaseAssignmentSyncService _syncService;

    public AdminCaseService(ICaseRepository caseRepository, ICaseAssignmentSyncService syncService)
    {
        _caseRepository = caseRepository;
        _syncService = syncService;
    }

    public async Task<List<CaseFormViewModel>> GetAllAsync()
    {
        var cases = await _caseRepository.GetAllAsync();
        return cases.Select(c => new CaseFormViewModel
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            Difficulty = c.Difficulty,
            IsPublished = c.IsPublished
        }).ToList();
    }

    public async Task<CaseFormViewModel?> GetByIdAsync(int id)
    {
        var c = await _caseRepository.GetByIdAsync(id);
        if (c == null) return null;

        return new CaseFormViewModel
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            Difficulty = c.Difficulty,
            IsPublished = c.IsPublished
        };
    }

    public async Task CreateAsync(CaseFormViewModel form)
    {
        var entity = new Case
        {
            Title = form.Title,
            Description = form.Description,
            Difficulty = form.Difficulty,
            IsPublished = form.IsPublished
        };

        await _caseRepository.CreateAsync(entity);

        if (entity.IsPublished)
        {
            await _syncService.AssignCaseToAllUsersAsync(entity.Id);
        }
    }

    public async Task UpdateAsync(CaseFormViewModel form)
    {
        var entity = await _caseRepository.GetByIdAsync(form.Id);
        if (entity == null) return;

        bool wasPublished = entity.IsPublished;

        entity.Title = form.Title;
        entity.Description = form.Description;
        entity.Difficulty = form.Difficulty;
        entity.IsPublished = form.IsPublished;

        await _caseRepository.UpdateAsync(entity);

        // If it just became published, sync assignments
        if (!wasPublished && entity.IsPublished)
        {
            await _syncService.AssignCaseToAllUsersAsync(entity.Id);
        }
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _caseRepository.GetByIdAsync(id);
        if (entity != null)
        {
            await _caseRepository.DeleteAsync(entity);
        }
    }

    public async Task PublishAsync(int id)
    {
        var entity = await _caseRepository.GetByIdAsync(id);
        if (entity == null) return;

        entity.IsPublished = true;
        await _caseRepository.UpdateAsync(entity);
        await _syncService.AssignCaseToAllUsersAsync(id);
    }
}