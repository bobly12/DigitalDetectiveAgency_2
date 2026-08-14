// Services/Interfaces/IAccusationService.cs
using DigitalDetectiveAgency.Models.DTOs;
using DigitalDetectiveAgency.Models.ViewModels;

namespace DigitalDetectiveAgency.Services.Interfaces;

public interface IAccusationService
{
    Task<AccusationFormViewModel?> GetAccusationFormAsync(int caseId, string userId);
    Task<bool> CanAccuseAsync(int caseId, string userId); // NEW
    Task<(bool Success, string? Error, AccusationResultViewModel? Result)> SubmitAccusationAsync(AccusationSubmitDto dto, string userId);
    Task<AccusationResultViewModel?> GetStoredResultAsync(int caseId, string userId); // NEW - re-view a completed case's result
}