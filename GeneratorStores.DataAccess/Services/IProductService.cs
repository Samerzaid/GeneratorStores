using GeneratorStores.DataAccess.Entities;

namespace GeneratorStores.DataAccess.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<Product> GetProductByIdAsync(int id);
    Task AddProductAsync(Product product);
    Task UpdateProductAsync(Product product);
    Task DeleteProductAsync(int id);
    Task<IEnumerable<Product>> GetNewArrivalsAsync(int count = 6);
    Task<IEnumerable<Product>> GetOldArrivalsAsync(int count = 6);


}

