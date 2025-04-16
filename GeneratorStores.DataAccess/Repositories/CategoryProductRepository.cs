using GeneratorStores.DataAccess.Entities;
using GeneratorStores.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GeneratorStores.DataAccess.Repositories;

public class CategoryProductRepository : ICategoryProductRepository
{
    private readonly GeneratorDbContext _context;

    public CategoryProductRepository(GeneratorDbContext context)
    {
        _context = context;
    }

    public async Task<CategoryProduct> GetByIdAsync(int id)
    {
        return await _context.CategoryProducts
            .Include(cp => cp.Category)
            .Include(cp => cp.Product)
            .FirstOrDefaultAsync(cp => cp.Id == id);
    }

    public async Task<IEnumerable<CategoryProduct>> GetByCategoryIdAsync(int categoryId)
    {
        return await _context.CategoryProducts
            .Where(cp => cp.CategoryId == categoryId)
            .Include(cp => cp.Product)
            .ToListAsync();
    }

    public async Task<IEnumerable<CategoryProduct>> GetAllAsync()
    {
        return await _context.CategoryProducts
            .Include(cp => cp.Category)
            .Include(cp => cp.Product)
            .ToListAsync();
    }

    public async Task AddAsync(CategoryProduct categoryProduct)
    {
        await _context.CategoryProducts.AddAsync(categoryProduct);
    }

    public async Task UpdateAsync(CategoryProduct categoryProduct)
    {
        _context.CategoryProducts.Update(categoryProduct);
    }

    public async Task DeleteAsync(int id)
    {
        var categoryProduct = await GetByIdAsync(id);
        if (categoryProduct != null)
        {
            _context.CategoryProducts.Remove(categoryProduct);
        }
    }
}

