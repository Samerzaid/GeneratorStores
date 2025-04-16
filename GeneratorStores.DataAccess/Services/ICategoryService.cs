using GeneratorStores.DataAccess.Dtos;
using GeneratorStores.DataAccess.Entities;

namespace GeneratorStores.DataAccess.Services;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task<Category> GetCategoryByIdAsync(int id);
    Task<IEnumerable<CategoryWithProductsDto>> GetCategoriesWithProductsAsync();
    Task AddCategoryAsync(Category category);
    Task UpdateCategoryAsync(Category category);
    Task DeleteCategoryAsync(int id);
    Task AddProductsToCategoryAsync(int categoryId, List<int> productIds);


}