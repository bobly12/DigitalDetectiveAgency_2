// Services/Implementations/AccusationService.cs
using DigitalDetectiveAgency.Models.DTOs;
using DigitalDetectiveAgency.Models.Entities;
using DigitalDetectiveAgency.Models.ViewModels;
using DigitalDetectiveAgency.Repositories.Interfaces;
using DigitalDetectiveAgency.Services.Interfaces;

namespace DigitalDetectiveAgency.Services.Implementations;

public class AccusationService : IAccusationService
{
    private readonly IAccusationRepository _accusationRepository;
    private readonly ICaseRepository _caseRepository;
    private readonly IBoardRepository _boardRepository;
    private readonly IScoringService _scoringService; // NEW

    public AccusationService(
        IAccusationRepository accusationRepository,
        ICaseRepository caseRepository,
        IBoardRepository boardRepository,
        IScoringService scoringService) // NEW
    {
        _accusationRepository = accusationRepository;
        _caseRepository = caseRepository;
        _boardRepository = boardRepository;
        _scoringService = scoringService; // NEW
    }

    public async Task<AccusationFormViewModel?> GetAccusationFormAsync(int caseId, string userId)
    {
        var playerCase = await _caseRepository.GetPlayerCaseAsync(caseId, userId);
        if (playerCase == null) return null;

        var existing = await _accusationRepository.GetByCaseAndUserAsync(caseId, userId);
        if (existing != null) return null;

        var suspects = await _caseRepository.GetSuspectsForCaseAsync(caseId);

        return new AccusationFormViewModel
        {
            CaseId = caseId,
            CaseTitle = playerCase.Case.Title,
            Suspects = suspects.Select(s => new SuspectOptionViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                ImageUrl = s.ImageUrl
            }).ToList()
        };
    }

    public async Task<(bool Success, string? Error, AccusationResultViewModel? Result)> SubmitAccusationAsync(AccusationSubmitDto dto, string userId)
    {
        var playerCase = await _caseRepository.GetPlayerCaseAsync(dto.CaseId, userId);
        if (playerCase == null)
            return (false, "You are not assigned to this case.", null);

        var existing = await _accusationRepository.GetByCaseAndUserAsync(dto.CaseId, userId);
        if (existing != null)
            return (false, "You have already submitted an accusation for this case.", null);

        var suspects = await _caseRepository.GetSuspectsForCaseAsync(dto.CaseId);
        var accusedSuspect = suspects.FirstOrDefault(s => s.Id == dto.SuspectId);
        if (accusedSuspect == null)
            return (false, "That suspect does not belong to this case.", null);

        var accusation = new Accusation
        {
            ApplicationUserId = userId,
            CaseId = dto.CaseId,
            AccusedSuspectId = dto.SuspectId
        };
        await _accusationRepository.AddAsync(accusation);

        // Calculate case strength (board connection accuracy)
        var connectionMatchPercent = await CalculateCaseStrengthAsync(dto.CaseId, userId);

        // NEW: determine correctness and final score
        bool wasCorrect = accusedSuspect.IsGuilty;
        int score = _scoringService.CalculateScore(wasCorrect, connectionMatchPercent);

        // Save completion + score in one call
        await _caseRepository.CompleteWithScoreAsync(playerCase, score);

        return (true, null, new AccusationResultViewModel
        {
            CaseId = dto.CaseId,
            CaseTitle = playerCase.Case.Title,
            AccusedSuspectName = accusedSuspect.Name,
            CaseStrengthPercent = connectionMatchPercent,
            WasCorrect = wasCorrect,   // NEW
            Score = score              // NEW
        });
    }

    private async Task<int> CalculateCaseStrengthAsync(int caseId, string userId)
    {
        var answerKey = await _boardRepository.GetAnswerKeyAsync(caseId);
        if (answerKey.Count == 0) return 0;

        var playerConnections = await _boardRepository.GetConnectionsAsync(caseId, userId);

        int matches = answerKey.Count(ak => playerConnections.Any(pc =>
            (pc.FromType == ak.FromType && pc.FromId == ak.FromId && pc.ToType == ak.ToType && pc.ToId == ak.ToId) ||
            (pc.FromType == ak.ToType && pc.FromId == ak.ToId && pc.ToType == ak.FromType && pc.ToId == ak.FromId)));

        return (int)Math.Round((double)matches / answerKey.Count * 100);
    }
}