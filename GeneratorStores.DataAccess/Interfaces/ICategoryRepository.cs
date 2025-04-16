using GeneratorStores.DataAccess.Entities;

namespace GeneratorStores.DataAccess.Interfaces
{
    public interface ICategoryRepository : IRepository<Category, int>
    {
        Task<IEnumerable<object>> GetCategoriesWithProductsAsync();
    }
}

