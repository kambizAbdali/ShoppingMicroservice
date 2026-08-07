using Discount.Core.Entities;

namespace Discount.Core.Interfaces
{
    public interface IDiscountRepository
    {
        Task<Coupon> GetDiscountByProductIdAsync(string productId);

        Task<Coupon> GetDiscountByProductNameAsync(string productName);

        Task<bool> CreateDiscountAsync(Coupon coupon);

        Task<bool> UpdateDiscountAsync(Coupon coupon);

        Task<bool> DeleteDiscountByProductIdAsync(string productId);

        Task<bool> DeleteDiscountByProductNameAsync(string productName);
    }
}