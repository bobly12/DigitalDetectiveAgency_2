// Controllers/CaseController.cs
using DigitalDetectiveAgency.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DigitalDetectiveAgency.Models.Entities;

namespace DigitalDetectiveAgency.Controllers;

[Authorize]
public class CaseController : Controller
{
    private readonly ICaseService _caseService;
    private readonly UserManager<ApplicationUser> _userManager;

    public CaseController(ICaseService caseService, UserManager<ApplicationUser> userManager)
    {
        _caseService = caseService;
        _userManager = userManager;
    }

    // GET: /Case
    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User)!;
        var cases = await _caseService.GetAssignedCasesAsync(userId);
        return View(cases);
    }

    // GET: /Case/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var userId = _userManager.GetUserId(User)!;
        var caseDetail = await _caseService.OpenCaseAsync(id, userId);

        if (caseDetail == null)  
        {
            return Forbid(); // Not assigned to this player
        }

        return View(caseDetail);
    }
    // CaseController.cs — add:
    public async Task<IActionResult> Intro(int id)
    {
        var userId = _userManager.GetUserId(User)!;
        var intro = await _caseService.GetCaseIntroAsync(id, userId);

        if (intro == null)
        {
            return Forbid();
        }

        return View(intro);
    }
}