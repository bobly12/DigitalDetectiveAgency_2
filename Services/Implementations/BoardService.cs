using DigitalDetectiveAgency.Models.DTOs;
using DigitalDetectiveAgency.Models.Entities;
using DigitalDetectiveAgency.Models.ViewModels;
using DigitalDetectiveAgency.Repositories.Interfaces;
using DigitalDetectiveAgency.Services.Interfaces;

namespace DigitalDetectiveAgency.Services.Implementations;

public class BoardService : IBoardService
{
    private readonly IBoardRepository _boardRepository;
    private readonly ICaseRepository _caseRepository;
    private readonly IInvestigationProgressService _progressService;

    private static readonly HashSet<string> ValidTypes = new()
    {
        "Evidence",
        "Suspect",
        "Witness"
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

        var evidence = await _caseRepository.GetEvidenceForCaseAsync(caseId);
        var suspects = await _caseRepository.GetSuspectsForCaseAsync(caseId);
        var witnesses = await _caseRepository.GetWitnessesForCaseAsync(caseId);
        var connections = await _boardRepository.GetConnectionsAsync(caseId, userId);
        var eliminatedIds = await _boardRepository.GetEliminatedSuspectIdsAsync(caseId, userId);

        var progress = await _progressService.GetInvestigationProgressAsync(caseId, userId);

        return new BoardViewModel
        {
            CaseId = caseId,
            CaseTitle = playerCase.Case.Title,
            CaseSummary = playerCase.Case.Description,

            VictimName = string.Empty,
            VictimOccupation = string.Empty,
            Location = string.Empty,

            IsCompleted = playerCase.IsCompleted,
            Score = playerCase.Score,

            Confidence = progress.Confidence,
            CanAccuse = progress.CanAccuse,
            PlayerConnections = progress.PlayerConnections,
            CorrectConnections = progress.CorrectConnections,
            TotalRequiredConnections = progress.TotalRequiredConnections,
            CorrectEliminatedSuspects = progress.CorrectEliminatedSuspects,
            TotalInnocentSuspects = progress.TotalInnocentSuspects,

            Evidence = evidence
                .Select(MapEvidence)
                .ToList(),

            Suspects = suspects
                .Select(s => MapSuspect(s, progress.UnlockedSuspectIds))
                .ToList(),

            Witnesses = witnesses
                .Select(MapWitness)
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

            EliminatedSuspectIds = eliminatedIds
        };
    }

    public async Task<(bool Success, string? Error, int ConnectionId)> SaveConnectionAsync(
        ConnectionRequestDto request,
        string userId)
    {
        var playerCase = await _caseRepository.GetPlayerCaseAsync(
            request.CaseId,
            userId);

        if (playerCase == null)
            return (false, "You are not assigned to this case.", 0);

        if (!ValidTypes.Contains(request.FromType) ||
            !ValidTypes.Contains(request.ToType))
        {
            return (false, "Invalid node type.", 0);
        }

        if (request.FromType == request.ToType &&
            request.FromId == request.ToId)
        {
            return (false, "Cannot connect a card to itself.", 0);
        }

        var exists = await _boardRepository.ConnectionExistsAsync(
            request.CaseId,
            userId,
            request.FromType,
            request.FromId,
            request.ToType,
            request.ToId);

        if (exists)
            return (false, "This connection already exists.", 0);

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
        // EF Core populates connection.Id after SaveChanges runs inside
        // AddConnectionAsync — the client needs this REAL id (not a fake
        // client-generated one) so a later DeleteConnection call for this
        // exact thread actually targets the right row.

        return (true, null, connection.Id);
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

        // Re-checks the unlock server-side on every call — the client never
        // gets to decide "I'm unlocked now, give me the text." Same rule
        // GetBoardAsync uses, same InvestigationProgressService snapshot.
        var progress = await _progressService.GetInvestigationProgressAsync(caseId, userId);
        if (!progress.UnlockedSuspectIds.Contains(suspectId))
            return (false, null, null);

        var suspects = await _caseRepository.GetSuspectsForCaseAsync(caseId);
        var suspect = suspects.FirstOrDefault(s => s.Id == suspectId);
        if (suspect == null)
            return (false, null, null);

        return (true, suspect.Motive, suspect.Alibi);
    }

    private static BoardNodeViewModel MapEvidence(Evidence evidence)
    {
        return new BoardNodeViewModel
        {
            Type = "Evidence",
            Id = evidence.Id,
            Name = evidence.Name,
            ImageUrl = evidence.ImageUrl ?? string.Empty,
            Description = evidence.Description ?? string.Empty
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
            IsAlibiUnlocked = unlocked
        };
    }

    private static BoardNodeViewModel MapWitness(Witness witness)
    {
        return new BoardNodeViewModel
        {
            Type = "Witness",
            Id = witness.Id,
            Name = witness.Name,
            ImageUrl = witness.ImageUrl ?? string.Empty,
            Description = witness.Statement ?? string.Empty
        };
    }
}
