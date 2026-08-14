using DigitalDetectiveAgency.Models.Entities;
using DigitalDetectiveAgency.Models.ViewModels;
using DigitalDetectiveAgency.Repositories.Interfaces;
using DigitalDetectiveAgency.Services.Interfaces;

namespace DigitalDetectiveAgency.Services.Implementations;

public class CaseService : ICaseService
{
    private readonly ICaseRepository _caseRepository;

    public CaseService(ICaseRepository caseRepository)
    {
        _caseRepository = caseRepository;
    }

    public async Task<List<CaseListItemViewModel>> GetAssignedCasesAsync(string userId)
    {
        var playerCases = await _caseRepository.GetAssignedCasesForUserAsync(userId);

        return playerCases.Select(pc => new CaseListItemViewModel
        {
            CaseId = pc.CaseId,
            Title = pc.Case.Title,
            Difficulty = pc.Case.Difficulty.ToString(),
            IsOpened = pc.IsOpened,
            IsCompleted = pc.IsCompleted,
            Score = pc.Score
        }).ToList();
    }

    public async Task<CaseDetailViewModel?> OpenCaseAsync(int caseId, string userId)
    {
        var playerCase = await _caseRepository.GetPlayerCaseAsync(caseId, userId);

        if (playerCase == null)
        {
            return null;
        }

        await _caseRepository.MarkOpenedAsync(playerCase);

        var evidence = await _caseRepository.GetEvidenceForCaseAsync(caseId);
        var suspects = await _caseRepository.GetSuspectsForCaseAsync(caseId);
        var witnesses = await _caseRepository.GetWitnessesForCaseAsync(caseId);

        return new CaseDetailViewModel
        {
            CaseId = playerCase.Case.Id,
            Title = playerCase.Case.Title,
            Description = playerCase.Case.Description,
            Difficulty = playerCase.Case.Difficulty.ToString(),
            IsCompleted = playerCase.IsCompleted,
            Score = playerCase.Score,
            Evidence = evidence.Select(e => new EvidenceViewModel
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                ImageUrl = e.ImageUrl
            }).ToList(),
            Suspects = suspects.Select(s => new SuspectViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                Motive = s.Motive,
                Alibi = s.Alibi,
                ImageUrl = s.ImageUrl
            }).ToList(),
            Witnesses = witnesses.Select(w => new WitnessViewModel
            {
                Id = w.Id,
                Name = w.Name,
                Statement = w.Statement,
                ImageUrl = w.ImageUrl
            }).ToList()
        };
    } // <--- Added the missing closing brace here for OpenCaseAsync!

    public async Task<CaseIntroViewModel?> GetCaseIntroAsync(int caseId, string userId)
    {
        var playerCase = await _caseRepository.GetPlayerCaseAsync(caseId, userId);
        if (playerCase == null) return null;

        var beats = new List<string>
        {
            playerCase.Case.Description
        };

        return new CaseIntroViewModel
        {
            CaseId = playerCase.Case.Id,
            Title = playerCase.Case.Title,
            StoryBeats = beats,
            Difficulty = playerCase.Case.Difficulty,
            TimeLimitSeconds = playerCase.Case.Difficulty == CaseDifficulty.Hard ? 600 : (int?)null,
            MaxAttempts = AccusationService.MaxAttempts
        };
    }
}