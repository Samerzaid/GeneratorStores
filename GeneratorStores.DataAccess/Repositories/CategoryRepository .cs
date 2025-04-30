using GeneratorStores.DataAccess.Dtos.ProductDtos;
using GeneratorStores.DataAccess.Dtos;
using GeneratorStores.DataAccess.Entities;
using GeneratorStores.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GeneratorStores.DataAccess.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly GeneratorDbContext _context;

        public CategoryRepository(GeneratorDbContext context)
        {
            _context = context;
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<IEnumerable<object>> GetCategoriesWithProductsAsync()
        {
            return await _context.Categories
                .Select(category => new CategoryWithProductsDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Products = _context.CategoryProducts
                        .Where(cp => cp.CategoryId == category.Id)
                        .Select(cp => new ProductDto
                        {
                            Id = cp.Product.Id,
                            ProductName = cp.Product.ProductName,
                            Price = cp.Product.Price
                        })
                        .ToList()
                })
                .ToListAsync();
        }


        public async Task AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
        }

        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
        }

        public async Task DeleteAsync(int id)
        {
            var category = await GetByIdAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }
        }


    }
}



