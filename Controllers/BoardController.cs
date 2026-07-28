// Controllers/BoardController.cs
using DigitalDetectiveAgency.Models.DTOs;
using DigitalDetectiveAgency.Models.Entities;
using DigitalDetectiveAgency.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DigitalDetectiveAgency.Controllers;

[Authorize]
public class BoardController : Controller
{
    private readonly IBoardService _boardService;
    private readonly IInvestigationProgressService _progressService;
    private readonly UserManager<ApplicationUser> _userManager;

    public BoardController(
        IBoardService boardService,
        IInvestigationProgressService progressService,
        UserManager<ApplicationUser> userManager)
    {
        _boardService = boardService;
        _progressService = progressService;
        _userManager = userManager;
    }

    // GET: /Board/Index/5
    public async Task<IActionResult> Index(int id)
    {
        var userId = _userManager.GetUserId(User)!;
        var board = await _boardService.GetBoardAsync(id, userId);

        if (board == null)
            return Forbid();

        return View(board);
    }

    // POST: /Board/SaveConnection
    [HttpPost]
    public async Task<IActionResult> SaveConnection([FromBody] ConnectionRequestDto request)
    {
        var userId = _userManager.GetUserId(User)!;
        var (success, error, connectionId) = await _boardService.SaveConnectionAsync(request, userId);

        if (!success)
            return BadRequest(new { message = error });

        var progress = await BuildProgressSnapshotAsync(request.CaseId, userId);

        return Ok(new
        {
            message = "Connection saved.",
            connectionId,
            progress
        });
    }

    // POST: /Board/DeleteConnection
    [HttpPost]
    public async Task<IActionResult> DeleteConnection([FromBody] DeleteConnectionRequestDto request)
    {
        var userId = _userManager.GetUserId(User)!;
        var (success, error) = await _boardService.DeleteConnectionAsync(request.ConnectionId, userId);

        if (!success)
            return BadRequest(new { message = error });

        var progress = await BuildProgressSnapshotAsync(request.CaseId, userId);

        return Ok(new
        {
            message = "Connection removed.",
            progress
        });
    }

    // POST: /Board/ToggleElimination
    [HttpPost]
    public async Task<IActionResult> ToggleElimination([FromBody] ToggleEliminationRequestDto request)
    {
        var userId = _userManager.GetUserId(User)!;
        var (success, error) = await _boardService.ToggleEliminationAsync(request, userId);

        if (!success)
            return BadRequest(new { message = error });

        var progress = await BuildProgressSnapshotAsync(request.CaseId, userId);

        return Ok(new
        {
            message = "Elimination toggled.",
            progress
        });
    }

    // GET: /Board/GetSuspectFile?caseId=5&suspectId=10
    // Only returns real Motive/Alibi if the suspect is actually unlocked —
    // checked fresh server-side on every call. Used by board.js right after
    // a connection unlocks a new suspect, instead of a full page reload.
    [HttpGet]
    public async Task<IActionResult> GetSuspectFile(int caseId, int suspectId)
    {
        var userId = _userManager.GetUserId(User)!;
        var (success, motive, alibi) = await _boardService.GetSuspectFileAsync(caseId, suspectId, userId);

        if (!success)
            return Forbid();

        return Ok(new { motive, alibi });
    }

    private async Task<object> BuildProgressSnapshotAsync(int caseId, string userId)
    {
        var progress = await _progressService.GetInvestigationProgressAsync(caseId, userId);

        return new
        {
            confidence = progress.Confidence,
            canAccuse = progress.CanAccuse,
            remainingConfidence = Math.Max(0, 75 - progress.Confidence),
            correctConnections = progress.CorrectConnections,
            totalRequiredConnections = progress.TotalRequiredConnections,
            correctEliminatedSuspects = progress.CorrectEliminatedSuspects,
            totalInnocentSuspects = progress.TotalInnocentSuspects,
            unlockedSuspectIds = progress.UnlockedSuspectIds
        };
    }
}
