using DigitalDetectiveAgency.Models.DTOs;
using DigitalDetectiveAgency.Models.ViewModels;

namespace DigitalDetectiveAgency.Services.Interfaces;

public interface IBoardService
{
    Task<BoardViewModel?> GetBoardAsync(int caseId, string userId);

    Task<(bool Success, string? Error, int ConnectionId, bool WasCorrect, string? Note)> SaveConnectionAsync(
        ConnectionRequestDto request,
        string userId);

    Task<int> GetWrongAttemptCountAsync(int caseId, string userId);

    Task<(bool Success, string? Error)> DeleteConnectionAsync(int connectionId, string userId);

    Task<(bool Success, string? Error)> ToggleEliminationAsync(ToggleEliminationRequestDto request, string userId);

    /// <summary>
    /// Returns a suspect's real Motive/Alibi ONLY if the player has actually
    /// unlocked them (per InvestigationProgressService). Used by the board's
    /// no-reload flow: the client never receives real Motive/Alibi text until
    /// this call confirms the unlock server-side — locked suspects' real text
    /// is never sent to the browser at all, same guarantee as before.
    /// </summary>
    Task<(bool Success, string? Motive, string? Alibi)> GetSuspectFileAsync(int caseId, int suspectId, string userId);

    /// <summary>
    /// Returns evidence details (Name, ImageUrl, Description) ONLY if the player
    /// has unlocked the piece of evidence. Prevents client-side exposure of locked leads.
    /// </summary>
    Task<(bool Success, string? Name, string? ImageUrl, string? Description)> GetEvidenceFileAsync(int caseId, int evidenceId, string userId);

    /// <summary>
    /// Returns witness details (Name, ImageUrl, Statement) ONLY if the player
    /// has unlocked the witness. Prevents client-side exposure of locked leads.
    /// </summary>
    Task<(bool Success, string? Name, string? ImageUrl, string? Description)> GetWitnessFileAsync(int caseId, int witnessId, string userId);
    
    // IBoardService.cs
    
}