using GeneratorStore.DataAccess.Interfaces;

namespace GeneratorStores.DataAccess.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IProductRepository Products { get; }
    IOrderRepository Orders { get; }
    ICategoryRepository Categories { get; }
    ICategoryProductRepository CategoryProducts { get; } // Add this
    IReviewRepository Reviews { get; } // Add this
    IBannerRepository Banners { get; }
    ICouponRepository Coupons { get; }


    Task<int> CompleteAsync();
}


