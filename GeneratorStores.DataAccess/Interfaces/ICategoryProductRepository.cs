using GeneratorStores.DataAccess.Entities;

namespace GeneratorStores.DataAccess.Interfaces;

public interface ICategoryProductRepository : IRepository<CategoryProduct, int>
{
    Task<IEnumerable<CategoryProduct>> GetByCategoryIdAsync(int categoryId);

    Task<CategoryProduct?> GetByCategoryAndProductIdAsync(int categoryId, int productId);

}



