using GeneratorStores.DataAccess.Entities;
using GeneratorStores.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GeneratorStores.DataAccess.Repositories;

public class ProductOrderRepository : IProductOrderRepository
{
    private readonly GeneratorDbContext _context;

    public ProductOrderRepository(GeneratorDbContext context)
    {
        _context = context;
    }

    public async Task<ProductOrder> GetByIdAsync(int id)
    {
        return await _context.ProductOrders
            .Include(po => po.Product)
            .Include(po => po.Order)
            .FirstOrDefaultAsync(po => po.Id == id);
    }

    public async Task<IEnumerable<ProductOrder>> GetAllAsync()
    {
        return await _context.ProductOrders
            .Include(po => po.Product)
            .Include(po => po.Order)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProductOrder>> GetProductOrdersByOrderIdAsync(int orderId)
    {
        return await _context.ProductOrders
            .Include(po => po.Product)
            .Where(po => po.OrderId == orderId)
            .ToListAsync();
    }

    public async Task AddAsync(ProductOrder productOrder)
    {
        await _context.ProductOrders.AddAsync(productOrder);
    }

    public async Task UpdateAsync(ProductOrder productOrder)
    {
        _context.ProductOrders.Update(productOrder);
    }

    public async Task DeleteAsync(int id)
    {
        var productOrder = await GetByIdAsync(id);
        if (productOrder != null)
        {
            _context.ProductOrders.Remove(productOrder);
        }
    }
}