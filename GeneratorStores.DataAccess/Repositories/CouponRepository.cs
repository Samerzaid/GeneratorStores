using GeneratorStores.DataAccess.Entities;
using GeneratorStores.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GeneratorStores.DataAccess.Repositories;

public class CouponRepository : ICouponRepository
{
    private readonly GeneratorDbContext _context;

    public CouponRepository(GeneratorDbContext context)
    {
        _context = context;
    }

    public async Task<Coupon> GetByIdAsync(int id)
    {
        return await _context.Coupons.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Coupon>> GetAllAsync()
    {
        return await _context.Coupons.ToListAsync();
    }

    public async Task AddAsync(Coupon coupon)
    {
        await _context.Coupons.AddAsync(coupon);
    }

    public async Task UpdateAsync(Coupon coupon)
    {
        var existing = await _context.Coupons.FindAsync(coupon.Id);
        if (existing != null)
        {
            _context.Entry(existing).CurrentValues.SetValues(coupon);
        }
    }

    public async Task DeleteAsync(int id)
    {
        var coupon = await GetByIdAsync(id);
        if (coupon != null)
        {
            _context.Coupons.Remove(coupon);
        }
    }

    public async Task<Coupon?> GetByCodeAsync(string code)
    {
        return await _context.Coupons.FirstOrDefaultAsync(c => c.Code == code);
    }
}