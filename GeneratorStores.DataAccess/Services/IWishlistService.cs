namespace GeneratorStores.DataAccess.Services;

public interface IWishlistService
{
    Task<HashSet<int>> GetWishlistAsync();
    Task<int> GetWishlistCountAsync();
    Task AddToWishlistAsync(int productId);
    Task RemoveFromWishlistAsync(int productId);
    Task<bool> IsInWishlistAsync(int productId);
}
