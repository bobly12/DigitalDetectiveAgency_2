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

    public async Task<Case?> GetCaseByIdAsync(int caseId)
    {
        return await _context.Cases.FindAsync(caseId);
    }

    public async Task<Case?> GetByIdAsync(int id)
    {
        return await GetCaseByIdAsync(id);
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

    public async Task CompleteWithScoreAsync(PlayerCase playerCase, int score)
    {
        playerCase.IsCompleted = true;
        playerCase.CompletedAt = DateTime.UtcNow;
        playerCase.Score = score;
        await _context.SaveChangesAsync();
    }

    public async Task SaveScoreAsync(PlayerCase playerCase, int score)
    {
        playerCase.Score = score;
        await _context.SaveChangesAsync();
    }

    public async Task<List<Case>> GetAllAsync() =>
        await _context.Cases.ToListAsync();

    public async Task<Case> CreateAsync(Case caseEntity)
    {
        _context.Cases.Add(caseEntity);
        await _context.SaveChangesAsync();
        return caseEntity;
    }

    public async Task UpdateAsync(Case caseEntity)
    {
        _context.Cases.Update(caseEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Case caseEntity)
    {
        _context.Cases.Remove(caseEntity);
        await _context.SaveChangesAsync();
    }

    public async Task<List<string>> GetAllUserIdsAsync() =>
        await _context.Users.Select(u => u.Id).ToListAsync();

    public async Task<bool> UserHasPlayerCaseAsync(string userId, int caseId) =>
        await _context.PlayerCases.AnyAsync(pc => pc.ApplicationUserId == userId && pc.CaseId == caseId);

    public async Task CreatePlayerCaseAsync(string userId, int caseId)
    {
        _context.PlayerCases.Add(new PlayerCase
        {
            ApplicationUserId = userId,
            CaseId = caseId
        });
        await _context.SaveChangesAsync();
    }

    public async Task<List<int>> GetPublishedCaseIdsAsync() =>
        await _context.Cases.Where(c => c.IsPublished).Select(c => c.Id).ToListAsync();
    
    public async Task<int?> GetFirstPublishedCaseIdAsync()
    {
        return await _context.Cases
            .Where(c => c.IsPublished)
            .OrderBy(c => c.Id)
            .Select(c => (int?)c.Id)
            .FirstOrDefaultAsync();
    }
}