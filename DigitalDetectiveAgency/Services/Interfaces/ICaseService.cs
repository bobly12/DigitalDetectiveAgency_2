// Services/Interfaces/ICaseService.cs
using DigitalDetectiveAgency.Models.ViewModels;

namespace DigitalDetectiveAgency.Services.Interfaces;

public interface ICaseService
{
    Task<List<CaseListItemViewModel>> GetAssignedCasesAsync(string userId);
    Task<CaseDetailViewModel?> OpenCaseAsync(int caseId, string userId);
    // ICaseService.cs — add:
    Task<CaseIntroViewModel?> GetCaseIntroAsync(int caseId, string userId);
}