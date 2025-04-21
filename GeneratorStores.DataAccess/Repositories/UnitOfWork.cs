using GeneratorStore.DataAccess.Interfaces;
using GeneratorStores.DataAccess.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace GeneratorStores.DataAccess.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly GeneratorDbContext _context;

    public IProductRepository Products { get; }
    public IOrderRepository Orders { get; }
    public IProductOrderRepository ProductOrders { get; }
    public ICategoryRepository Categories { get; } // Add the Categories repository
    public ICategoryProductRepository CategoryProducts { get; } // Add this
    public IReviewRepository Reviews { get; } // Add this
    public IBannerRepository Banners { get; } // Add this
    public ICouponRepository Coupons { get; } // Add this

    public UnitOfWork(GeneratorDbContext context)
    {
        _context = context;
        Products = new ProductRepository(context);
        Orders = new OrderRepository(context);
        ProductOrders = new ProductOrderRepository(context);
        Categories = new CategoryRepository(context); // Instantiate CategoryRepository
        CategoryProducts = new CategoryProductRepository(context);
        Reviews = new ReviewRepository(context); // Initialize ReviewRepository
        Banners = new BannerRepository(context);
        Coupons = new CouponRepository(context);

    }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}


