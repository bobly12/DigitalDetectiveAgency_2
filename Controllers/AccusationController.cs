// Controllers/AccusationController.cs
using DigitalDetectiveAgency.Models.DTOs;
using DigitalDetectiveAgency.Models.Entities;
using DigitalDetectiveAgency.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DigitalDetectiveAgency.Controllers;

[Authorize]
public class AccusationController : Controller
{
    private readonly IAccusationService _accusationService;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccusationController(IAccusationService accusationService, UserManager<ApplicationUser> userManager)
    {
        _accusationService = accusationService;
        _userManager = userManager;
    }

    // GET: /Accusation/Create/5
    public async Task<IActionResult> Create(int id)
    {
        var userId = _userManager.GetUserId(User)!;

        // NEW: block direct-URL access to the form before enough evidence is gathered
        var canAccuse = await _accusationService.CanAccuseAsync(id, userId);
        if (!canAccuse)
        {
            TempData["BoardMessage"] = "You haven't gathered enough evidence to make an accusation yet.";
            return RedirectToAction("Index", "Board", new { id });
        }

        var form = await _accusationService.GetAccusationFormAsync(id, userId);

        if (form == null)
            return Forbid();

        return View(form);
    }

    // POST: /Accusation/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AccusationSubmitDto dto)
    {
        var userId = _userManager.GetUserId(User)!;
        var (success, error, result) = await _accusationService.SubmitAccusationAsync(dto, userId);

        if (!success)
        {
            ModelState.AddModelError(string.Empty, error!);
            var form = await _accusationService.GetAccusationFormAsync(dto.CaseId, userId);
            return View(form);
        }

        return View("Result", result);
    }
}