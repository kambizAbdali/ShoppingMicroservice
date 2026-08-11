using Ordering.Core.Entities;

namespace Ordering.Core.Repositories
{
    /// <summary>
    /// قرارداد Repository مربوط به سفارش‌ها
    /// </summary>
    public interface IOrderRepository : IGenericRepository<Order>
    {
        /// <summary>
        /// دریافت تمام سفارش‌های مربوط به یک کاربر
        /// </summary>
        Task<IEnumerable<Order>> GetOrdersByUserNameAsync(string userName);
    }
}
