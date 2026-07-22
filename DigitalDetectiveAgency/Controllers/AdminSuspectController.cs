// Controllers/AdminSuspectController.cs
using DigitalDetectiveAgency.Models.ViewModels;
using DigitalDetectiveAgency.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalDetectiveAgency.Controllers;

[Authorize(Roles = "Admin")]
[Route("Admin/Suspect")]
public class AdminSuspectController : Controller
{
    private readonly IAdminSuspectService _service;

    public AdminSuspectController(IAdminSuspectService service) => _service = service;

    [HttpGet("Create/{caseId}")]
    public IActionResult Create(int caseId) => View(new SuspectFormViewModel { CaseId = caseId });

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SuspectFormViewModel form)
    {
        if (!ModelState.IsValid) return View(form);
        await _service.CreateAsync(form);
        return RedirectToAction("Edit", "AdminCase", new { id = form.CaseId });
    }

    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        var form = await _service.GetByIdAsync(id);
        if (form == null) return NotFound();
        return View(form);
    }

    [HttpPost("Edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, SuspectFormViewModel form)
    {
        if (id != form.Id) return BadRequest();
        if (!ModelState.IsValid) return View(form);

        await _service.UpdateAsync(form);
        return RedirectToAction("Edit", "AdminCase", new { id = form.CaseId });
    }

    [HttpPost("Delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, int caseId)
    {
        await _service.DeleteAsync(id);
        return RedirectToAction("Edit", "AdminCase", new { id = caseId });
    }
}