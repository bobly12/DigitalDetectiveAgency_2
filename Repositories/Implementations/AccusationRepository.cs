// Repositories/Implementations/AccusationRepository.cs
using DigitalDetectiveAgency.Data;
using DigitalDetectiveAgency.Models.Entities;
using DigitalDetectiveAgency.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DigitalDetectiveAgency.Repositories.Implementations;

public class AccusationRepository : IAccusationRepository
{
    private readonly ApplicationDbContext _context;

    public AccusationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Most recent attempt for this case/user (used to check "already solved").
    public async Task<Accusation?> GetByCaseAndUserAsync(int caseId, string userId)
    {
        return await _context.Accusations
            .Include(a => a.AccusedSuspect)
            .Where(a => a.CaseId == caseId && a.ApplicationUserId == userId)
            .OrderByDescending(a => a.SubmittedAt)
            .FirstOrDefaultAsync();
    }

    // NEW - full attempt history, oldest first, so the UI can show a tries trail.
    public async Task<List<Accusation>> GetAllByCaseAndUserAsync(int caseId, string userId)
    {
        return await _context.Accusations
            .Include(a => a.AccusedSuspect)
            .Where(a => a.CaseId == caseId && a.ApplicationUserId == userId)
            .OrderBy(a => a.SubmittedAt)
            .ToListAsync();
    }

    // NEW - how many tries the player has already burned on this case.
    public async Task<int> GetAttemptCountAsync(int caseId, string userId)
    {
        return await _context.Accusations
            .CountAsync(a => a.CaseId == caseId && a.ApplicationUserId == userId);
    }

    public async Task<Accusation> AddAsync(Accusation accusation)
    {
        _context.Accusations.Add(accusation);
        await _context.SaveChangesAsync();
        return accusation;
    }
}