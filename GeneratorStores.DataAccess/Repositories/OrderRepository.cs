using GeneratorStore.DataAccess.Interfaces;
using GeneratorStores.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace GeneratorStores.DataAccess.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly GeneratorDbContext _context;

    public OrderRepository(GeneratorDbContext context)
    {
        _context = context;
    }

    public async Task<Order> GetByIdAsync(int id)
    {
        return await _context.Orders
            .Include(o => o.ProductsInOrder)
            .ThenInclude(po => po.Product)
            .Include(o => o.User) // Include ApplicationUser
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _context.Orders
            .Include(o => o.ProductsInOrder)
            .ThenInclude(po => po.Product)
            .Include(o => o.User) // Include ApplicationUser
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetOrdersWithProductsAsync()
    {
        return await _context.Orders
            .Include(o => o.ProductsInOrder)
            .ThenInclude(po => po.Product)
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
    {
        return await _context.Orders
            .Include(o => o.ProductsInOrder)
            .ThenInclude(po => po.Product)
            .Where(o => o.UserId == userId)
            .ToListAsync();
    }

    public async Task AddAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
    }

    public async Task UpdateAsync(Order order)
    {
        _context.Orders.Update(order);
    }

    public async Task DeleteAsync(int id)
    {
        var order = await GetByIdAsync(id);
        if (order != null)
        {
            _context.Orders.Remove(order);
        }
    }
}


