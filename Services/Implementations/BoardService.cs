using DigitalDetectiveAgency.Models.DTOs;
using DigitalDetectiveAgency.Models.Entities;
using DigitalDetectiveAgency.Models.ViewModels;
using DigitalDetectiveAgency.Repositories.Interfaces;
using DigitalDetectiveAgency.Services.Interfaces;

namespace DigitalDetectiveAgency.Services.Implementations;

public class BoardService : IBoardService
{
    // NEW - tries system: max wrong tries allowed per string connection (specific pair)
    // before that pair locks out and can't be tried again.
    public const int MaxConnectionTries = 3;

    private readonly IBoardRepository _boardRepository;
    private readonly ICaseRepository _caseRepository;
    private readonly IInvestigationProgressService _progressService;

    private static readonly HashSet<string> ValidTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "Evidence",
        "Suspect",
        "Witness",
        "Location",
        "Clue"
    };

    public BoardService(
        IBoardRepository boardRepository,
        ICaseRepository caseRepository,
        IInvestigationProgressService progressService)
    {
        _boardRepository = boardRepository;
        _caseRepository = caseRepository;
        _progressService = progressService;
    }

    public async Task<BoardViewModel?> GetBoardAsync(int caseId, string userId)
    {
        var playerCase = await _caseRepository.GetPlayerCaseAsync(caseId, userId);

        if (playerCase == null)
            return null;

        // FIX: stamp OpenedAt here too, since the Board can be reached without
        // ever hitting CaseService.OpenCaseAsync (e.g. via the Intro screen).
        await _caseRepository.MarkOpenedAsync(playerCase);

        var evidence = await _caseRepository.GetEvidenceForCaseAsync(caseId);
        var suspects = await _caseRepository.GetSuspectsForCaseAsync(caseId);
        var witnesses = await _caseRepository.GetWitnessesForCaseAsync(caseId);
        var connections = await _boardRepository.GetConnectionsAsync(caseId, userId);
        var eliminatedIds = await _boardRepository.GetEliminatedSuspectIdsAsync(caseId, userId);
        var attempts = await _boardRepository.GetAttemptsAsync(caseId, userId);

        var progress = await _progressService.GetInvestigationProgressAsync(caseId, userId);

        return new BoardViewModel
        {
            CaseId = caseId,
            CaseTitle = playerCase.Case.Title,
            CaseSummary = playerCase.Case.Description,

            VictimName = playerCase.Case.VictimName,
            VictimOccupation = playerCase.Case.VictimOccupation,
            Location = playerCase.Case.Location,
            Difficulty = playerCase.Case.Difficulty.ToString(),
            TimeLimitSeconds = playerCase.Case.Difficulty == CaseDifficulty.Hard ? 600 : (int?)null,
            
            // FIX: Explicitly specify DateTimeKind.Utc so .ToString("o") appends 'Z'.
            // Prevents browsers in local timezones (e.g. UTC+8) from misinterpreting naive SQLite datetimes.
            OpenedAtUtc = DateTime.SpecifyKind(playerCase.OpenedAt ?? DateTime.UtcNow, DateTimeKind.Utc),

            IsCompleted = playerCase.IsCompleted,
            Score = playerCase.Score,

            Confidence = progress.Confidence,
            CanAccuse = progress.CanAccuse,
            PlayerConnections = progress.PlayerConnections,
            CorrectConnections = progress.CorrectConnections,
            TotalRequiredConnections = progress.TotalRequiredConnections,
            CorrectEliminatedSuspects = progress.CorrectEliminatedSuspects,
            TotalInnocentSuspects = progress.TotalInnocentSuspects,
            NextFocusHint = progress.NextFocusHint,

            Evidence = evidence
                .Select(e => MapEvidence(e, progress.UnlockedEvidenceIds))
                .ToList(),

            Suspects = suspects
                .Select(s => MapSuspect(s, progress.UnlockedSuspectIds))
                .ToList(),

            Witnesses = witnesses
                .Select(w => MapWitness(w, progress.UnlockedWitnessIds))
                .ToList(),

            Connections = connections
                .Select(c => new BoardConnectionViewModel
                {
                    Id = c.Id,
                    FromType = c.FromType,
                    FromId = c.FromId,
                    ToType = c.ToType,
                    ToId = c.ToId
                })
                .ToList(),

            EliminatedSuspectIds = eliminatedIds,

            TriedWrongPairs = attempts
                .Where(a => !a.WasCorrect)
                .Select(a => new
                {
                    Key = string.CompareOrdinal($"{a.FromType}{a.FromId}", $"{a.ToType}{a.ToId}") <= 0
                        ? $"{a.FromType}{a.FromId}|{a.ToType}{a.ToId}"
                        : $"{a.ToType}{a.ToId}|{a.FromType}{a.FromId}",
                    Attempt = a
                })
                .GroupBy(x => x.Key)
                .Select(g => new BoardTriedPairViewModel
                {
                    FromType = g.First().Attempt.FromType,
                    FromId = g.First().Attempt.FromId,
                    ToType = g.First().Attempt.ToType,
                    ToId = g.First().Attempt.ToId,
                    WrongTryCount = g.Count(),
                    IsLocked = g.Count() >= MaxConnectionTries
                })
                .ToList()
        };
    }

    public async Task<(bool Success, string? Error, int ConnectionId, bool WasCorrect, string? Note)> SaveConnectionAsync(
        ConnectionRequestDto request,
        string userId)
    {
        var playerCase = await _caseRepository.GetPlayerCaseAsync(request.CaseId, userId);
        if (playerCase == null)
            return (false, "You are not assigned to this case.", 0, false, null);

        if (!ValidTypes.Contains(request.FromType) || !ValidTypes.Contains(request.ToType))
            return (false, "Invalid node type.", 0, false, null);

        if (request.FromType == request.ToType && request.FromId == request.ToId)
            return (false, "Cannot connect a card to itself.", 0, false, null);

        var exists = await _boardRepository.ConnectionExistsAsync(
            request.CaseId, userId, request.FromType, request.FromId, request.ToType, request.ToId);
        if (exists)
            return (false, "This connection already exists.", 0, false, null);

        // NEW - 3-tries cap per specific pair (regardless of direction)
        var pastAttempts = await _boardRepository.GetAttemptsAsync(request.CaseId, userId);
        int wrongTriesForThisPair = pastAttempts.Count(a =>
            !a.WasCorrect &&
            ((a.FromType == request.FromType && a.FromId == request.FromId && a.ToType == request.ToType && a.ToId == request.ToId) ||
             (a.FromType == request.ToType && a.FromId == request.ToId && a.ToType == request.FromType && a.ToId == request.FromId)));

        if (wrongTriesForThisPair >= MaxConnectionTries)
            return (false, "You're out of tries for this connection.", 0, false, null);

        var answerKey = await _boardRepository.GetAnswerKeyAsync(request.CaseId);

        // Fixed bidirectional match logic typo
        var match = answerKey.FirstOrDefault(correct =>
            (correct.FromType == request.FromType && correct.FromId == request.FromId &&
             correct.ToType == request.ToType && correct.ToId == request.ToId)
            ||
            (correct.FromType == request.ToType && correct.FromId == request.ToId &&
             correct.ToType == request.FromType && correct.ToId == request.FromId));

        bool isCorrect = match != null;

        await _boardRepository.LogAttemptAsync(new ConnectionAttempt
        {
            ApplicationUserId = userId,
            CaseId = request.CaseId,
            FromType = request.FromType,
            FromId = request.FromId,
            ToType = request.ToType,
            ToId = request.ToId,
            WasCorrect = isCorrect
        });

        if (match == null)
            return (false, "These aren't connected.", 0, false, null);

        var connection = new ClueConnection
        {
            ApplicationUserId = userId,
            CaseId = request.CaseId,
            FromType = request.FromType,
            FromId = request.FromId,
            ToType = request.ToType,
            ToId = request.ToId
        };

        await _boardRepository.AddConnectionAsync(connection);

        return (true, null, connection.Id, true, match.Note);
    }

    public async Task<(bool Success, string? Error)> DeleteConnectionAsync(
        int connectionId,
        string userId)
    {
        var connection = await _boardRepository.GetConnectionByIdAsync(
            connectionId,
            userId);

        if (connection == null)
            return (false, "Connection not found.");

        await _boardRepository.DeleteConnectionAsync(connection);

        return (true, null);
    }

    public async Task<(bool Success, string? Error)> ToggleEliminationAsync(
        ToggleEliminationRequestDto request,
        string userId)
    {
        var playerCase = await _caseRepository.GetPlayerCaseAsync(
            request.CaseId,
            userId);

        if (playerCase == null)
            return (false, "You are not assigned to this case.");

        await _boardRepository.ToggleEliminationAsync(
            request.CaseId,
            request.SuspectId,
            userId);

        return (true, null);
    }

    public async Task<(bool Success, string? Motive, string? Alibi)> GetSuspectFileAsync(
        int caseId,
        int suspectId,
        string userId)
    {
        var playerCase = await _caseRepository.GetPlayerCaseAsync(caseId, userId);
        if (playerCase == null)
            return (false, null, null);

        var progress = await _progressService.GetInvestigationProgressAsync(caseId, userId);
        if (!progress.UnlockedSuspectIds.Contains(suspectId))
            return (false, null, null);

        var suspects = await _caseRepository.GetSuspectsForCaseAsync(caseId);
        var suspect = suspects.FirstOrDefault(s => s.Id == suspectId);
        if (suspect == null)
            return (false, null, null);

        return (true, suspect.Motive, suspect.Alibi);
    }

    public async Task<(bool Success, string? Name, string? ImageUrl, string? Description)> GetEvidenceFileAsync(
        int caseId,
        int evidenceId,
        string userId)
    {
        var playerCase = await _caseRepository.GetPlayerCaseAsync(caseId, userId);
        if (playerCase == null)
            return (false, null, null, null);

        var progress = await _progressService.GetInvestigationProgressAsync(caseId, userId);
        if (!progress.UnlockedEvidenceIds.Contains(evidenceId))
            return (false, null, null, null);

        var evidenceList = await _caseRepository.GetEvidenceForCaseAsync(caseId);
        var evidence = evidenceList.FirstOrDefault(e => e.Id == evidenceId);
        if (evidence == null)
            return (false, null, null, null);

        return (true, evidence.Name, evidence.ImageUrl, evidence.Description);
    }

    public async Task<(bool Success, string? Name, string? ImageUrl, string? Description)> GetWitnessFileAsync(
        int caseId,
        int witnessId,
        string userId)
    {
        var playerCase = await _caseRepository.GetPlayerCaseAsync(caseId, userId);
        if (playerCase == null)
            return (false, null, null, null);

        var progress = await _progressService.GetInvestigationProgressAsync(caseId, userId);
        if (!progress.UnlockedWitnessIds.Contains(witnessId))
            return (false, null, null, null);

        var witnessList = await _caseRepository.GetWitnessesForCaseAsync(caseId);
        var witness = witnessList.FirstOrDefault(w => w.Id == witnessId);
        if (witness == null)
            return (false, null, null, null);

        return (true, witness.Name, witness.ImageUrl, witness.Statement);
    }

    public async Task<int> GetWrongAttemptCountAsync(int caseId, string userId)
    {
        return await _boardRepository.GetWrongAttemptCountAsync(caseId, userId);
    }

    private static BoardNodeViewModel MapEvidence(
        Evidence evidence,
        HashSet<int> unlockedEvidenceIds)
    {
        bool revealed = unlockedEvidenceIds.Contains(evidence.Id);

        return new BoardNodeViewModel
        {
            Type = "Evidence",
            Id = evidence.Id,
            Name = revealed ? evidence.Name : "Undiscovered Lead",
            ImageUrl = revealed ? (evidence.ImageUrl ?? string.Empty) : string.Empty,
            Description = revealed ? (evidence.Description ?? string.Empty) : string.Empty,
            IsRevealed = revealed
        };
    }

    private static BoardNodeViewModel MapSuspect(
        Suspect suspect,
        HashSet<int> unlockedSuspectIds)
    {
        bool unlocked = unlockedSuspectIds.Contains(suspect.Id);

        return new BoardNodeViewModel
        {
            Type = "Suspect",
            Id = suspect.Id,
            Name = suspect.Name,
            ImageUrl = suspect.ImageUrl ?? string.Empty,
            Description = suspect.Description ?? string.Empty,

            Motive = unlocked
                ? suspect.Motive ?? string.Empty
                : "???",

            Alibi = unlocked
                ? suspect.Alibi ?? string.Empty
                : "???",

            IsMotiveUnlocked = unlocked,
            IsAlibiUnlocked = unlocked,
            IsRevealed = true
        };
    }

    private static BoardNodeViewModel MapWitness(
        Witness witness,
        HashSet<int> unlockedWitnessIds)
    {
        bool revealed = unlockedWitnessIds.Contains(witness.Id);

        return new BoardNodeViewModel
        {
            Type = "Witness",
            Id = witness.Id,
            Name = revealed ? witness.Name : "Undiscovered Lead",
            ImageUrl = revealed ? (witness.ImageUrl ?? string.Empty) : string.Empty,
            Description = revealed ? (witness.Statement ?? string.Empty) : string.Empty,
            IsRevealed = revealed
        };
    }

    public async Task ResetProgressAsync(int caseId, string userId)
    {
        await _boardRepository.ResetProgressAsync(caseId, userId);
    }
}