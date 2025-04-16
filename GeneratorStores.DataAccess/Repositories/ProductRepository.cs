using GeneratorStore.DataAccess.Interfaces;
using GeneratorStores.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace GeneratorStores.DataAccess.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly GeneratorDbContext _context;

    public ProductRepository(GeneratorDbContext context)
    {
        _context = context;
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
    }

    public async Task UpdateAsync(Product product)
    {
        var existingProduct = await _context.Products.FindAsync(product.Id);
        if (existingProduct != null)
        {
            _context.Entry(existingProduct).CurrentValues.SetValues(product);
        }
        else
        {
            throw new Exception("Product not found in the database.");
        }
    }


    public async Task DeleteAsync(int id)
    {
        var product = await GetByIdAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
        }
    }
}


