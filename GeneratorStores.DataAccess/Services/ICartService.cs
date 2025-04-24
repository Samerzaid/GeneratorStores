using GeneratorStores.DataAccess.Entities;

namespace GeneratorStores.DataAccess.Services;

public interface ICartService
{
    event Action CartChanged; // Add this event

    Task AddToCart(Product product);
    Task RemoveFromCart(int productId);
    Task<IEnumerable<Product>> GetCartItems();
    Task ClearCart();
}


