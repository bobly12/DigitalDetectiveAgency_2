using DigitalDetectiveAgency.Data;
using DigitalDetectiveAgency.Models.Entities;
using DigitalDetectiveAgency.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DigitalDetectiveAgency.Repositories.Implementations;

public class BoardRepository : IBoardRepository
{
    private readonly ApplicationDbContext _context;

    public BoardRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ClueConnection>> GetConnectionsAsync(int caseId, string userId)
    {
        return await _context.ClueConnections
            .Where(cc => cc.CaseId == caseId && cc.ApplicationUserId == userId)
            .ToListAsync();
    }

    public async Task<ClueConnection> AddConnectionAsync(ClueConnection connection)
    {
        _context.ClueConnections.Add(connection);
        await _context.SaveChangesAsync();
        return connection;
    }

    public async Task<bool> ConnectionExistsAsync(int caseId, string userId, string fromType, int fromId, string toType, int toId)
    {
        return await _context.ClueConnections.AnyAsync(cc =>
            cc.CaseId == caseId && cc.ApplicationUserId == userId &&
            ((cc.FromType == fromType && cc.FromId == fromId && cc.ToType == toType && cc.ToId == toId) ||
             (cc.FromType == toType && cc.FromId == toId && cc.ToType == fromType && cc.ToId == fromId)));
    }

    public async Task<ClueConnection?> GetConnectionByIdAsync(int connectionId, string userId)
    {
        return await _context.ClueConnections
            .FirstOrDefaultAsync(cc => cc.Id == connectionId && cc.ApplicationUserId == userId);
    }

    public async Task DeleteConnectionAsync(ClueConnection connection)
    {
        _context.ClueConnections.Remove(connection);
        await _context.SaveChangesAsync();
    }

    public async Task<List<CaseConnection>> GetAnswerKeyAsync(int caseId)
    {
        return await _context.CaseConnections
            .Where(cc => cc.CaseId == caseId)
            .ToListAsync();
    }

    // NEW: Suspect Elimination methods
    public async Task<List<int>> GetEliminatedSuspectIdsAsync(int caseId, string userId)
    {
        return await _context.SuspectEliminations
            .Where(se => se.CaseId == caseId && se.ApplicationUserId == userId)
            .Select(se => se.SuspectId)
            .ToListAsync();
    }

    public async Task ToggleEliminationAsync(int caseId, int suspectId, string userId)
    {
        var existing = await _context.SuspectEliminations
            .FirstOrDefaultAsync(se => se.CaseId == caseId && se.SuspectId == suspectId && se.ApplicationUserId == userId);

        if (existing != null)
        {
            _context.SuspectEliminations.Remove(existing);
        }
        else
        {
            _context.SuspectEliminations.Add(new SuspectElimination
            {
                CaseId = caseId,
                SuspectId = suspectId,
                ApplicationUserId = userId
            });
        }

        await _context.SaveChangesAsync();
    }

    public async Task LogAttemptAsync(ConnectionAttempt attempt)
    {
        _context.ConnectionAttempts.Add(attempt);
        await _context.SaveChangesAsync();
    }

    public async Task<List<ConnectionAttempt>> GetAttemptsAsync(int caseId, string userId)
    {
        return await _context.ConnectionAttempts
            .Where(a => a.CaseId == caseId && a.ApplicationUserId == userId)
            .ToListAsync();
    }

    public async Task<int> GetWrongAttemptCountAsync(int caseId, string userId)
    {
        return await _context.ConnectionAttempts
            .Where(a => a.CaseId == caseId && a.ApplicationUserId == userId && !a.WasCorrect)
            .CountAsync();
    }

    public async Task ResetProgressAsync(int caseId, string userId)
    {
        var clues = _context.ClueConnections.Where(c => c.CaseId == caseId && c.ApplicationUserId == userId);
        var eliminations = _context.SuspectEliminations.Where(e => e.CaseId == caseId && e.ApplicationUserId == userId);
        var attempts = _context.ConnectionAttempts.Where(a => a.CaseId == caseId && a.ApplicationUserId == userId);

        _context.ClueConnections.RemoveRange(clues);
        _context.SuspectEliminations.RemoveRange(eliminations);
        _context.ConnectionAttempts.RemoveRange(attempts);

        // FIX: reset the timer anchor too — otherwise a reset case (e.g. after
        // "Time's Up") reopens with the same stale OpenedAt and expires instantly
        // again, looping forever.
        var playerCase = await _context.PlayerCases
            .FirstOrDefaultAsync(pc => pc.CaseId == caseId && pc.ApplicationUserId == userId);

        if (playerCase != null)
        {
            playerCase.OpenedAt = DateTime.UtcNow;
            playerCase.IsCompleted = false;
            playerCase.Score = 0;
        }

        await _context.SaveChangesAsync();
    }
}