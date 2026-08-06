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
    private readonly IScoringService _scoringService;
    private readonly IInvestigationProgressService _progressService;

    public AccusationService(
        IAccusationRepository accusationRepository,
        ICaseRepository caseRepository,
        IBoardRepository boardRepository,
        IScoringService scoringService,
        IInvestigationProgressService progressService)
    {
        _accusationRepository = accusationRepository;
        _caseRepository = caseRepository;
        _boardRepository = boardRepository;
        _scoringService = scoringService;
        _progressService = progressService;
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

    /// <summary>
    /// Checks whether the player currently meets the confidence threshold to accuse.
    /// Used by AccusationController's GET action to block direct-URL access to the
    /// form before enough evidence has been gathered, using the same single
    /// calculation path as the POST gate below and the Board's progress meter.
    /// </summary>
    public async Task<bool> CanAccuseAsync(int caseId, string userId)
    {
        var progress = await _progressService.GetInvestigationProgressAsync(caseId, userId);
        return progress.CanAccuse;
    }

    public async Task<(bool Success, string? Error, AccusationResultViewModel? Result)> SubmitAccusationAsync(AccusationSubmitDto dto, string userId)
    {
        var playerCase = await _caseRepository.GetPlayerCaseAsync(dto.CaseId, userId);
        if (playerCase == null)
            return (false, "You are not assigned to this case.", null);

        var existing = await _accusationRepository.GetByCaseAndUserAsync(dto.CaseId, userId);
        if (existing != null)
            return (false, "You have already submitted an accusation for this case.", null);

        // Gate on investigation progress before allowing an accusation.
        // Same InvestigationProgressService snapshot the Board page reads from,
        // so there is exactly one definition of "ready to accuse."
        var progress = await _progressService.GetInvestigationProgressAsync(dto.CaseId, userId);
        if (!progress.CanAccuse)
            return (false, "You haven't gathered enough evidence to make an accusation.", null);

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

        bool wasCorrect = accusedSuspect.IsGuilty;
        int score = _scoringService.CalculateScore(wasCorrect, connectionMatchPercent);

        // Save completion + score in one call
        await _caseRepository.CompleteWithScoreAsync(playerCase, score);

        var summary = BuildDetectiveSummary(
            playerCase.Case.Title,
            accusedSuspect.Name,
            wasCorrect,
            connectionMatchPercent,
            score);

        return (true, null, new AccusationResultViewModel
        {
            CaseId = dto.CaseId,
            CaseTitle = playerCase.Case.Title,
            AccusedSuspectName = accusedSuspect.Name,
            AccusedSuspectImageUrl = accusedSuspect.ImageUrl, // NEW - feeds the mugshot/stamp animation
            CaseStrengthPercent = connectionMatchPercent,
            WasCorrect = wasCorrect,
            Score = score,
            DetectiveSummary = summary
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

    private static string BuildDetectiveSummary(
        string caseTitle,
        string accusedName,
        bool wasCorrect,
        int connectionMatchPercent,
        int score)
    {
        var opening = wasCorrect
            ? $"The trail led straight to {accusedName}."
            : $"You closed the file on {accusedName} — but the real story didn't add up.";

        var boardLine = connectionMatchPercent switch
        {
            >= 90 => "Every thread on the board held. This was a clean, airtight case.",
            >= 70 => "Most of the connections held up, though a few threads were left loose.",
            >= 40 => "The board told part of the story, but too many links were guesswork.",
            _ => "The corkboard barely resembled the real chain of events."
        };

        var closing = wasCorrect
            ? "Case closed."
            : "The case remains open, for now.";

        return $"{opening} {boardLine} {closing}";
    }
}