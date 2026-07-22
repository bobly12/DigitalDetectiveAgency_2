// Repositories/Implementations/EvidenceRepository.cs
using DigitalDetectiveAgency.Data;
using DigitalDetectiveAgency.Models.Entities;
using DigitalDetectiveAgency.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DigitalDetectiveAgency.Repositories.Implementations;

public class EvidenceRepository : IEvidenceRepository
{
    private readonly ApplicationDbContext _context;
    public EvidenceRepository(ApplicationDbContext context) => _context = context;

    public async Task<Evidence?> GetByIdAsync(int id) =>
        await _context.EvidenceItems.FirstOrDefaultAsync(e => e.Id == id);

    public async Task<Evidence> CreateAsync(Evidence evidence)
    {
        _context.EvidenceItems.Add(evidence);
        await _context.SaveChangesAsync();
        return evidence;
    }

    public async Task UpdateAsync(Evidence evidence)
    {
        _context.EvidenceItems.Update(evidence);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Evidence evidence)
    {
        _context.EvidenceItems.Remove(evidence);
        await _context.SaveChangesAsync();
    }
}