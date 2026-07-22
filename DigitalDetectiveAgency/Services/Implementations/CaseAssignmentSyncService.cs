// Services/Implementations/CaseAssignmentSyncService.cs
using DigitalDetectiveAgency.Repositories.Interfaces;
using DigitalDetectiveAgency.Services.Interfaces;

namespace DigitalDetectiveAgency.Services.Implementations;

public class CaseAssignmentSyncService : ICaseAssignmentSyncService
{
    private readonly ICaseRepository _caseRepository;

    public CaseAssignmentSyncService(ICaseRepository caseRepository)
    {
        _caseRepository = caseRepository;
    }

    public async Task AssignCaseToAllUsersAsync(int caseId)
    {
        var userIds = await _caseRepository.GetAllUserIdsAsync();

        foreach (var userId in userIds)
        {
            var alreadyAssigned = await _caseRepository.UserHasPlayerCaseAsync(userId, caseId);
            if (!alreadyAssigned)
            {
                await _caseRepository.CreatePlayerCaseAsync(userId, caseId);
            }
        }
    }

    public async Task AssignAllPublishedCasesToUserAsync(string userId)
    {
        var publishedCaseIds = await _caseRepository.GetPublishedCaseIdsAsync();

        foreach (var caseId in publishedCaseIds)
        {
            var alreadyAssigned = await _caseRepository.UserHasPlayerCaseAsync(userId, caseId);
            if (!alreadyAssigned)
            {
                await _caseRepository.CreatePlayerCaseAsync(userId, caseId);
            }
        }
    }
}