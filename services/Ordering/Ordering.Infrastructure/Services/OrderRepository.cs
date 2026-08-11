using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;

namespace Ordering.Infrastructure.Services
{
    /// <summary>
    /// Repository اختصاصی مربوط به سفارش‌ها
    /// </summary>
    public class OrderRepository(OrderContext context)
        : GenericRepository<Order>(context), IOrderRepository
    {
        private readonly OrderContext _context = context;

        /// <summary>
        /// دریافت سفارش‌های فعال بر اساس نام کاربری
        /// </summary>
        public async Task<IEnumerable<Order>> GetOrdersByUserNameAsync(
            string userName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(userName);

            return await _context.Orders
                .AsNoTracking()
                .Where(order =>
                    order.UserName == userName &&
                    order.IsActive)
                .OrderByDescending(order => order.CreatedDate)
                .ToListAsync();
        }
    }
}
