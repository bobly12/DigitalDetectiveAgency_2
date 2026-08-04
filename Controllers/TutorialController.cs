using DigitalDetectiveAgency.Models.Entities;
using DigitalDetectiveAgency.Models.ViewModels;
using DigitalDetectiveAgency.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DigitalDetectiveAgency.Controllers;

[Authorize]
public class TutorialController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICaseRepository _caseRepository;

    public TutorialController(UserManager<ApplicationUser> userManager, ICaseRepository caseRepository)
    {
        _userManager = userManager;
        _caseRepository = caseRepository;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
            return Challenge();

        var firstCaseId = await _caseRepository.GetFirstPublishedCaseIdAsync();

        if (firstCaseId == null)
        {
            // No published case exists yet — nothing to send the player to.
            TempData["BoardMessage"] = "No cases are available yet. Check back soon, Detective.";
            return RedirectToAction("Index", "Case");
        }

        if (user.HasCompletedTutorial)
            return RedirectToAction("Index", "Board", new { id = firstCaseId });

        var vm = new TutorialViewModel
        {
            Speaker = "Chief Investigator",
            Title = "Detective Training",
            FirstCaseId = firstCaseId.Value,
            AllowSkip = true,
            Dialogue = new List<string>
            {
                "Welcome, Detective.",
                "Before you begin your first investigation, you must learn how the Investigation Board works.",
                "Click the red pin on one card.",
                "Then click another pin to connect the evidence.",
                "Correct investigation increases your Investigation Confidence.",
                "Reach 75% confidence before making an accusation.",
                "Good luck, Detective."
            }
        };

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Complete()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
            return Unauthorized();

        user.HasCompletedTutorial = true;
        await _userManager.UpdateAsync(user);

        var firstCaseId = await _caseRepository.GetFirstPublishedCaseIdAsync();

        return Ok(new
        {
            redirect = firstCaseId == null
                ? Url.Action("Index", "Case")
                : Url.Action("Index", "Board", new { id = firstCaseId })
        });
    }
}