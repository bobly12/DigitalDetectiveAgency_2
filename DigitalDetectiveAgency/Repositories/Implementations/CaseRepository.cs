using DigitalDetectiveAgency.Data;
using DigitalDetectiveAgency.Models.Entities;
using DigitalDetectiveAgency.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DigitalDetectiveAgency.Repositories.Implementations;

public class CaseRepository : ICaseRepository
{
    private readonly ApplicationDbContext _context;

    public CaseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Case?> GetByIdAsync(int id)
    {
        return await _context.Cases.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<PlayerCase>> GetAssignedCasesForUserAsync(string userId)
    {
        return await _context.PlayerCases
            .Include(pc => pc.Case)
            .Where(pc => pc.ApplicationUserId == userId)
            .ToListAsync();
    }

    public async Task<PlayerCase?> GetPlayerCaseAsync(int caseId, string userId)
    {
        return await _context.PlayerCases
            .Include(pc => pc.Case)
            .FirstOrDefaultAsync(pc => pc.CaseId == caseId && pc.ApplicationUserId == userId);
    }

    public async Task MarkOpenedAsync(PlayerCase playerCase)
    {
        if (!playerCase.IsOpened)
        {
            playerCase.IsOpened = true;
            playerCase.OpenedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Evidence>> GetEvidenceForCaseAsync(int caseId)
    {
        return await _context.EvidenceItems
            .Where(e => e.CaseId == caseId)
            .ToListAsync();
    }
    // CaseRepository.cs — add:
    public async Task<List<Suspect>> GetSuspectsForCaseAsync(int caseId)
    {
        return await _context.Suspects
            .Where(s => s.CaseId == caseId)
            .ToListAsync();
    }

    public async Task<List<Witness>> GetWitnessesForCaseAsync(int caseId)
    {
        return await _context.Witnesses
            .Where(w => w.CaseId == caseId)
            .ToListAsync();
    }
}