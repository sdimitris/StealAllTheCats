using Microsoft.EntityFrameworkCore;
using StealAllTheCats.Domain.Entities;
using StealAllTheCats.Domain.Repositories;
using StealAllTheCats.Infrastructure.Database;

namespace StealAllTheCats.Infrastructure.Repositories;

public class CatRepository : ICatRepository
{
    private readonly AppDbContext _context;
    
    public CatRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CatEntity>> GetCatsAsync(int page, int pageSize)
    {
        return await _context.Cats.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
    }

    public async Task<CatEntity> GetCatByIdAsync(int id)
    {
        return await _context.Cats.FindAsync(id);
    }

    public async Task AddCatAsync(CatEntity cat)
    {
        await _context.Cats.AddAsync(cat);
    }

    public async Task<bool> CatExistsAsync(string catId)
    {
        return await _context.Cats.AnyAsync(c => c.CatId == catId);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}