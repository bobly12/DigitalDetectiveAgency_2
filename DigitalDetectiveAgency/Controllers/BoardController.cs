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
    private readonly UserManager<ApplicationUser> _userManager;

    public BoardController(IBoardService boardService, UserManager<ApplicationUser> userManager)
    {
        _boardService = boardService;
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
        var (success, error) = await _boardService.SaveConnectionAsync(request, userId);

        if (!success)
            return BadRequest(new { message = error });

        return Ok(new { message = "Connection saved." });
    }

    // POST: /Board/DeleteConnection
    [HttpPost]
    public async Task<IActionResult> DeleteConnection([FromBody] DeleteConnectionRequestDto request)
    {
        var userId = _userManager.GetUserId(User)!;
        var (success, error) = await _boardService.DeleteConnectionAsync(request.ConnectionId, userId);

        if (!success)
            return BadRequest(new { message = error });

        return Ok(new { message = "Connection removed." });
    }
}