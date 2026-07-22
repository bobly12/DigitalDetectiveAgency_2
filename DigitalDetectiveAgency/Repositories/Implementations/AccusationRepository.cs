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

    public async Task<Accusation?> GetByCaseAndUserAsync(int caseId, string userId)
    {
        return await _context.Accusations
            .Include(a => a.AccusedSuspect)
            .FirstOrDefaultAsync(a => a.CaseId == caseId && a.ApplicationUserId == userId);
    }

    public async Task<Accusation> AddAsync(Accusation accusation)
    {
        _context.Accusations.Add(accusation);
        await _context.SaveChangesAsync();
        return accusation;
    }
}