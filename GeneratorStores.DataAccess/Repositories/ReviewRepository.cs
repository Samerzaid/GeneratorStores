using GeneratorStores.DataAccess.Entities;
using GeneratorStores.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GeneratorStores.DataAccess.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly GeneratorDbContext _context;

    public ReviewRepository(GeneratorDbContext context)
    {
        _context = context;
    }

    public async Task<Review> GetByIdAsync(int id)
    {
        return await _context.Reviews
            .Include(r => r.Product)
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<Review>> GetAllAsync()
    {
        return await _context.Reviews
            .Include(r => r.Product)
            .Include(r => r.User)
            .ToListAsync();
    }

    public async Task<IEnumerable<Review>> GetReviewsByProductIdAsync(int productId)
    {
        return await _context.Reviews
            .Where(r => r.ProductId == productId)
            .Include(r => r.User)
            .ToListAsync();
    }

    public async Task<IEnumerable<Review>> GetReviewsByUserIdAsync(string userId)
    {
        return await _context.Reviews
            .Where(r => r.UserId == userId)
            .Include(r => r.Product)
            .ToListAsync();
    }

    public async Task AddAsync(Review review)
    {
        await _context.Reviews.AddAsync(review);
    }

    public async Task UpdateAsync(Review review)
    {
        _context.Reviews.Update(review);
    }

    public async Task DeleteAsync(int id)
    {
        var review = await GetByIdAsync(id);
        if (review != null)
        {
            _context.Reviews.Remove(review);
        }
    }
}

