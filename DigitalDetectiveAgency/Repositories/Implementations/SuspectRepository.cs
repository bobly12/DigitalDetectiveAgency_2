// Repositories/Implementations/SuspectRepository.cs
using DigitalDetectiveAgency.Data;
using DigitalDetectiveAgency.Models.Entities;
using DigitalDetectiveAgency.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DigitalDetectiveAgency.Repositories.Implementations;

public class SuspectRepository : ISuspectRepository
{
    private readonly ApplicationDbContext _context;
    public SuspectRepository(ApplicationDbContext context) => _context = context;

    public async Task<Suspect?> GetByIdAsync(int id) =>
        await _context.Suspects.FirstOrDefaultAsync(s => s.Id == id);

    public async Task<Suspect> CreateAsync(Suspect suspect)
    {
        _context.Suspects.Add(suspect);
        await _context.SaveChangesAsync();
        return suspect;
    }

    public async Task UpdateAsync(Suspect suspect)
    {
        _context.Suspects.Update(suspect);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Suspect suspect)
    {
        _context.Suspects.Remove(suspect);
        await _context.SaveChangesAsync();
    }
}