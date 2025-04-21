using GeneratorStores.DataAccess.Entities;

namespace GeneratorStores.DataAccess.Services;

public class CartService : ICartService
{
    private readonly List<Product> _cart = new();

    public event Action? CartChanged;

    public Task AddToCart(Product product)
    {
        _cart.Add(product);
        NotifyCartChanged();
        return Task.CompletedTask;
    }

    public Task RemoveFromCart(int productId)
    {
        var product = _cart.FirstOrDefault(p => p.Id == productId);
        if (product != null)
        {
            _cart.Remove(product);
            NotifyCartChanged();
        }
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Product>> GetCartItems()
    {
        return Task.FromResult(_cart.AsEnumerable());
    }

    public Task ClearCart()
    {
        _cart.Clear();
        NotifyCartChanged();
        return Task.CompletedTask;
    }

    private void NotifyCartChanged()
    {
        CartChanged?.Invoke();
    }
}


