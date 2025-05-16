using GeneratorStores.DataAccess.Entities;

namespace GeneratorStores.DataAccess.Interfaces;

public interface IUserRepository
{
    Task<ApplicationUser?> GetUserByIdAsync(string userId);
    Task<string?> GetUserFullNameAsync(string userId);
}