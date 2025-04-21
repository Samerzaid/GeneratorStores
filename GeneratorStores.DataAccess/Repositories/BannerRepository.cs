using GeneratorStores.DataAccess.Entities;
using GeneratorStores.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GeneratorStores.DataAccess.Repositories;

public class BannerRepository : IBannerRepository
{
    private readonly GeneratorDbContext _context;

    public BannerRepository(GeneratorDbContext context)
    {
        _context = context;
    }

    public async Task<Banner> GetByIdAsync(int id)
    {
        return await _context.Banners.FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<IEnumerable<Banner>> GetAllAsync()
    {
        return await _context.Banners.ToListAsync();
    }

    public async Task AddAsync(Banner banner)
    {
        await _context.Banners.AddAsync(banner);
    }

    public async Task UpdateAsync(Banner banner)
    {
        var existing = await _context.Banners.FindAsync(banner.Id);
        if (existing != null)
        {
            _context.Entry(existing).CurrentValues.SetValues(banner);
        }
    }

    public async Task DeleteAsync(int id)
    {
        var banner = await GetByIdAsync(id);
        if (banner != null)
        {
            _context.Banners.Remove(banner);
        }
    }
}