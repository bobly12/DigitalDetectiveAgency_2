// Services/Interfaces/IBoardService.cs
using DigitalDetectiveAgency.Models.DTOs;
using DigitalDetectiveAgency.Models.ViewModels;

namespace DigitalDetectiveAgency.Services.Interfaces;

public interface IBoardService
{
    Task<BoardViewModel?> GetBoardAsync(int caseId, string userId);
    Task<(bool Success, string? Error)> SaveConnectionAsync(ConnectionRequestDto request, string userId);
    Task<(bool Success, string? Error)> DeleteConnectionAsync(int connectionId, string userId);
    Task<(bool Success, string? Error)> ToggleEliminationAsync(ToggleEliminationRequestDto request, string userId);
}