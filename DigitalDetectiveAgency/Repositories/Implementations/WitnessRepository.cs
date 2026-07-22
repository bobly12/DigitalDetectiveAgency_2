// Repositories/Implementations/WitnessRepository.cs
using DigitalDetectiveAgency.Data;
using DigitalDetectiveAgency.Models.Entities;
using DigitalDetectiveAgency.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DigitalDetectiveAgency.Repositories.Implementations;

public class WitnessRepository : IWitnessRepository
{
    private readonly ApplicationDbContext _context;
    public WitnessRepository(ApplicationDbContext context) => _context = context;

    public async Task<Witness?> GetByIdAsync(int id) =>
        await _context.Witnesses.FirstOrDefaultAsync(w => w.Id == id);

    public async Task<Witness> CreateAsync(Witness witness)
    {
        _context.Witnesses.Add(witness);
        await _context.SaveChangesAsync();
        return witness;
    }

    public async Task UpdateAsync(Witness witness)
    {
        _context.Witnesses.Update(witness);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Witness witness)
    {
        _context.Witnesses.Remove(witness);
        await _context.SaveChangesAsync();
    }
}