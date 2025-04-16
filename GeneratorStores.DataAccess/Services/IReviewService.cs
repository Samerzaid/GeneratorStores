using GeneratorStores.DataAccess.Dtos;
using GeneratorStores.DataAccess.Entities;

namespace GeneratorStores.DataAccess.Services;

public interface IReviewService
{
    Task<IEnumerable<Review>> GetAllReviewsAsync();
    Task<IEnumerable<Review>> GetReviewsByProductIdAsync(int productId);
    Task<IEnumerable<Review>> GetReviewsByUserIdAsync(string userId);
    Task AddReviewAsync(Review review);
    Task UpdateReviewAsync(Review review);
    Task DeleteReviewAsync(int id);

}

