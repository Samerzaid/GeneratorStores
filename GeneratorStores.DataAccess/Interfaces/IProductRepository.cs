using GeneratorStores.DataAccess.Entities;
using GeneratorStores.DataAccess.Interfaces;

namespace GeneratorStore.DataAccess.Interfaces;

public interface IProductRepository : IRepository<Product, int>
{
    Task<IEnumerable<Product>> GetAllAsync();

}



