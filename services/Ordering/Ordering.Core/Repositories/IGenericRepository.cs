using Ordering.Core.Common;
using System.Linq.Expressions;

namespace Ordering.Core.Repositories
{
    /// <summary>
    /// قرارداد عمومی برای عملیات مربوط به Repositoryها
    /// </summary>
    public interface IGenericRepository<T>
        where T : BaseEntity
    {
        /// <summary>
        /// افزودن یک موجودیت جدید
        /// </summary>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// حذف یک موجودیت
        /// </summary>
        Task<T?> DeleteAsync(T entity);

        /// <summary>
        /// بررسی وجود حداقل یک رکورد مطابق شرط
        /// </summary>
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);

        /// <summary>
        /// دریافت تمام رکوردهای فعال
        /// </summary>
        Task<IReadOnlyList<T>> GetAllAsync();

        /// <summary>
        /// دریافت رکوردها بر اساس شرط
        /// </summary>
        Task<IReadOnlyList<T>> GetAllAsync(
            Expression<Func<T, bool>> expression);

        /// <summary>
        /// دریافت یک رکورد بر اساس شناسه
        /// </summary>
        Task<T?> GetByIdAsync(long id);

        /// <summary>
        /// ویرایش یک موجودیت
        /// </summary>
        Task<T> UpdateAsync(T entity);
    }
}
