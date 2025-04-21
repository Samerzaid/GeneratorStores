using GeneratorStores.DataAccess.Entities;

namespace GeneratorStores.DataAccess.Interfaces;

public interface ICouponRepository : IRepository<Coupon, int>
{
    Task<Coupon?> GetByCodeAsync(string code);
}