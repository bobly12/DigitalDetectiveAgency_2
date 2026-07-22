// Services/Interfaces/ICaseAssignmentSyncService.cs
namespace DigitalDetectiveAgency.Services.Interfaces;

public interface ICaseAssignmentSyncService
{
    Task AssignCaseToAllUsersAsync(int caseId);
    Task AssignAllPublishedCasesToUserAsync(string userId);
}