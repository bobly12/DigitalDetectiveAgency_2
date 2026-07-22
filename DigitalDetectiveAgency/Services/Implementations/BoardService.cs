// Services/Implementations/BoardService.cs
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

    private static readonly HashSet<string> ValidTypes = new() { "Evidence", "Suspect", "Witness" };

    public BoardService(IBoardRepository boardRepository, ICaseRepository caseRepository)
    {
        _boardRepository = boardRepository;
        _caseRepository = caseRepository;
    }

    public async Task<BoardViewModel?> GetBoardAsync(int caseId, string userId)
    {
        var playerCase = await _caseRepository.GetPlayerCaseAsync(caseId, userId);
        if (playerCase == null) return null; // not assigned to this player

        var evidence = await _caseRepository.GetEvidenceForCaseAsync(caseId);
        var suspects = await _caseRepository.GetSuspectsForCaseAsync(caseId);
        var witnesses = await _caseRepository.GetWitnessesForCaseAsync(caseId);
        var connections = await _boardRepository.GetConnectionsAsync(caseId, userId);

        return new BoardViewModel
        {
            CaseId = caseId,
            CaseTitle = playerCase.Case.Title,
            Evidence = evidence.Select(e => new BoardNodeViewModel { Type = "Evidence", Id = e.Id, Name = e.Name, ImageUrl = e.ImageUrl }).ToList(),
            Suspects = suspects.Select(s => new BoardNodeViewModel { Type = "Suspect", Id = s.Id, Name = s.Name, ImageUrl = s.ImageUrl }).ToList(),
            Witnesses = witnesses.Select(w => new BoardNodeViewModel { Type = "Witness", Id = w.Id, Name = w.Name, ImageUrl = w.ImageUrl }).ToList(),
            Connections = connections.Select(c => new BoardConnectionViewModel
            {
                Id = c.Id, FromType = c.FromType, FromId = c.FromId, ToType = c.ToType, ToId = c.ToId
            }).ToList()
        };
    }

    public async Task<(bool Success, string? Error)> SaveConnectionAsync(ConnectionRequestDto request, string userId)
    {
        // Validate the player is actually assigned to this case
        var playerCase = await _caseRepository.GetPlayerCaseAsync(request.CaseId, userId);
        if (playerCase == null)
            return (false, "You are not assigned to this case.");

        // Validate node types are one of the three allowed
        if (!ValidTypes.Contains(request.FromType) || !ValidTypes.Contains(request.ToType))
            return (false, "Invalid node type.");

        // Prevent connecting a node to itself
        if (request.FromType == request.ToType && request.FromId == request.ToId)
            return (false, "Cannot connect a card to itself.");

        // Prevent duplicate connections (either direction)
        var exists = await _boardRepository.ConnectionExistsAsync(
            request.CaseId, userId, request.FromType, request.FromId, request.ToType, request.ToId);
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

    public async Task<(bool Success, string? Error)> DeleteConnectionAsync(int connectionId, string userId)
    {
        var connection = await _boardRepository.GetConnectionByIdAsync(connectionId, userId);
        if (connection == null)
            return (false, "Connection not found.");

        await _boardRepository.DeleteConnectionAsync(connection);
        return (true, null);
    }
}