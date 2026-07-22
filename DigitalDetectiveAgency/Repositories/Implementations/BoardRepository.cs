// Repositories/Implementations/BoardRepository.cs
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
    // BoardRepository.cs — add:
    public async Task<List<CaseConnection>> GetAnswerKeyAsync(int caseId)
    {
        return await _context.CaseConnections
            .Where(cc => cc.CaseId == caseId)
            .ToListAsync();
    }
}