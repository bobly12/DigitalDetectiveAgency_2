using DigitalDetectiveAgency.Models.Entities;
using DigitalDetectiveAgency.Models.ViewModels;
using DigitalDetectiveAgency.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DigitalDetectiveAgency.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICaseRepository _caseRepository;

    public ProfileController(UserManager<ApplicationUser> userManager, ICaseRepository caseRepository)
    {
        _userManager = userManager;
        _caseRepository = caseRepository;
    }

    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User)!;
        var user = await _userManager.GetUserAsync(User);
        var playerCases = await _caseRepository.GetAssignedCasesForUserAsync(userId);

        var completed = playerCases.Where(pc => pc.IsCompleted).ToList();
        var avgStrength = completed.Any() ? (int)completed.Average(pc => pc.Score ?? 0) : 0;

        var viewModel = new ProfileViewModel
        {
            DetectiveName = user!.DetectiveName,
            Email = user.Email!,
            CasesSolved = completed.Count,
            AverageScore = avgStrength
        };

        return View(viewModel);
    }
}