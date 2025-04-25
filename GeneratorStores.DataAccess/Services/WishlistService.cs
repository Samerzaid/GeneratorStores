using Microsoft.JSInterop;

namespace GeneratorStores.DataAccess.Services;


public class WishlistService : IWishlistService
{
    private readonly IJSRuntime _jsRuntime;

    public WishlistService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<int> GetWishlistCountAsync()
    {
        return await _jsRuntime.InvokeAsync<int>("wishlist.count");
    }

    public async Task<HashSet<int>> GetWishlistAsync()
    {
        var ids = await _jsRuntime.InvokeAsync<int[]>("wishlist.get");
        return ids.ToHashSet();
    }

    public async Task AddToWishlistAsync(int productId)
    {
        await _jsRuntime.InvokeVoidAsync("wishlist.add", productId);
    }

    public async Task RemoveFromWishlistAsync(int productId)
    {
        await _jsRuntime.InvokeVoidAsync("wishlist.remove", productId);
    }

    public async Task<bool> IsInWishlistAsync(int productId)
    {
        var ids = await GetWishlistAsync();
        return ids.Contains(productId);
    }
}
