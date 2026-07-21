// Controllers/AdminCaseController.cs
using DigitalDetectiveAgency.Models.ViewModels;
using DigitalDetectiveAgency.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalDetectiveAgency.Controllers;

[Authorize(Roles = "Admin")]
[Route("Admin/Case")]
public class AdminCaseController : Controller
{
    private readonly IAdminCaseService _adminCaseService;

    public AdminCaseController(IAdminCaseService adminCaseService)
    {
        _adminCaseService = adminCaseService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var cases = await _adminCaseService.GetAllAsync();
        return View(cases);
    }

    [HttpGet("Create")]
    public IActionResult Create() => View(new CaseFormViewModel());

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CaseFormViewModel form)
    {
        if (!ModelState.IsValid) return View(form);

        await _adminCaseService.CreateAsync(form);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        var form = await _adminCaseService.GetByIdAsync(id);
        if (form == null) return NotFound();
        return View(form);
    }

    [HttpPost("Edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CaseFormViewModel form)
    {
        if (id != form.Id) return BadRequest();
        if (!ModelState.IsValid) return View(form);

        await _adminCaseService.UpdateAsync(form);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("Delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _adminCaseService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("Publish/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Publish(int id)
    {
        await _adminCaseService.PublishAsync(id);
        return RedirectToAction(nameof(Index));
    }
}