using GeneratorStores.DataAccess.Entities;

namespace GeneratorStores.DataAccess.Interfaces;

public interface IReviewRepository : IRepository<Review, int>
{
    Task<IEnumerable<Review>> GetReviewsByProductIdAsync(int productId);
    Task<IEnumerable<Review>> GetReviewsByUserIdAsync(string userId);
}

