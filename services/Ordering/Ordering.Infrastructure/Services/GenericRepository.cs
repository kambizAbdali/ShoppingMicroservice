using Microsoft.EntityFrameworkCore;
using Ordering.Core.Common;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;
using System.Linq.Expressions;

namespace Ordering.Infrastructure.Services
{
    /// <summary>
    /// پیاده‌سازی عمومی Repository برای موجودیت‌های دامنه
    /// </summary>
    public class GenericRepository<T>(OrderContext context)
        : IGenericRepository<T>
        where T : BaseEntity
    {
        private readonly DbSet<T> _dbSet = context.Set<T>();

        /// <summary>
        /// افزودن موجودیت جدید به Context
        /// </summary>
        public async Task<T> AddAsync(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            await _dbSet.AddAsync(entity);
            
            // ذخیره اطلاعات در دیتابیس در سطح Unit of Work انجام می‌شود
            return entity;
        }

        /// <summary>
        /// بررسی وجود رکورد مطابق شرط
        /// </summary>
        public async Task<bool> AnyAsync(
            Expression<Func<T, bool>> expression)
        {
            ArgumentNullException.ThrowIfNull(expression);

            return await _dbSet
                .AsNoTracking()
                .AnyAsync(expression);
        }

        /// <summary>
        /// حذف موجودیت
        /// </summary>
        public Task<T?> DeleteAsync(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            _dbSet.Remove(entity);

            // SaveChanges در لایه Unit of Work یا DbContext انجام می‌شود
            return Task.FromResult(entity);
        }

        /// <summary>
        /// دریافت تمام موجودیت‌های فعال
        /// </summary>
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .Where(entity => entity.IsActive)
                .ToListAsync();
        }

        /// <summary>
        /// دریافت موجودیت‌های فعال مطابق شرط
        /// </summary>
        public async Task<IReadOnlyList<T>> GetAllAsync(
            Expression<Func<T, bool>> expression)
        {
            ArgumentNullException.ThrowIfNull(expression);

            return await _dbSet
                .AsNoTracking()
                .Where(entity => entity.IsActive)
                .Where(expression)
                .ToListAsync();
        }

        /// <summary>
        /// دریافت موجودیت فعال بر اساس شناسه
        /// </summary>
        public async Task<T?> GetByIdAsync(long id)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(entity =>
                    entity.Id == id &&
                    entity.IsActive);
        }  

        /// <summary>
        /// علامت‌گذاری موجودیت برای ویرایش
        /// </summary>
        public Task<T> UpdateAsync(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            _dbSet.Update(entity);

            // تغییرات بعداً با SaveChangesAsync ذخیره می‌شوند
            return Task.FromResult(entity);
        }
    }
}
