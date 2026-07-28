using DigitalDetectiveAgency.Models.Entities;
using DigitalDetectiveAgency.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DigitalDetectiveAgency.Controllers;

[Authorize]
public class TutorialController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public TutorialController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
            return Challenge();

        if (user.HasCompletedTutorial)
            return RedirectToAction("Index", "Board", new { id = 1 });

        var vm = new TutorialViewModel
        {
            Speaker = "Chief Investigator",
            Title = "Detective Training",
            FirstCaseId = 1,
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

        return Ok(new
        {
            redirect = Url.Action("Index", "Board", new { id = 1 })
        });
    }
}