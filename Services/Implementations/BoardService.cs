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

    private static readonly HashSet<string> ValidTypes = new()
    {
        "Evidence",
        "Suspect",
        "Witness"
    };

    public BoardService(
        IBoardRepository boardRepository,
        ICaseRepository caseRepository)
    {
        _boardRepository = boardRepository;
        _caseRepository = caseRepository;
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

        return new BoardViewModel
        {
            CaseId = caseId,
            CaseTitle = playerCase.Case.Title,
            CaseSummary = playerCase.Case.Description, // Mapped to Case.Description
            VictimName = string.Empty,                 // Fallback if not on entity model
            VictimOccupation = string.Empty,           // Fallback if not on entity model
            Location = string.Empty,                   // Fallback if not on entity model
            IsCompleted = playerCase.IsCompleted,
            Score = playerCase.Score,

            Evidence = evidence
                .Select(MapEvidence)
                .ToList(),

            Suspects = suspects
                .Select(MapSuspect)
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

    public async Task<(bool Success, string? Error)> SaveConnectionAsync(
        ConnectionRequestDto request,
        string userId)
    {
        var playerCase = await _caseRepository.GetPlayerCaseAsync(
            request.CaseId,
            userId);

        if (playerCase == null)
            return (false, "You are not assigned to this case.");

        if (!ValidTypes.Contains(request.FromType) ||
            !ValidTypes.Contains(request.ToType))
        {
            return (false, "Invalid node type.");
        }

        if (request.FromType == request.ToType &&
            request.FromId == request.ToId)
        {
            return (false, "Cannot connect a card to itself.");
        }

        var exists = await _boardRepository.ConnectionExistsAsync(
            request.CaseId,
            userId,
            request.FromType,
            request.FromId,
            request.ToType,
            request.ToId);

        if (exists)
            return (false, "This connection already exists.");

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

        return (true, null);
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

    private static BoardNodeViewModel MapSuspect(Suspect suspect)
    {
        return new BoardNodeViewModel
        {
            Type = "Suspect",
            Id = suspect.Id,
            Name = suspect.Name,
            ImageUrl = suspect.ImageUrl ?? string.Empty,
            Description = suspect.Description ?? string.Empty,
            Motive = suspect.Motive ?? string.Empty,
            Alibi = suspect.Alibi ?? string.Empty
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