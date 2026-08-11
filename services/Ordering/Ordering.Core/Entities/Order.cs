using Ordering.Core.Common;

namespace Ordering.Core.Entities
{
    /// <summary>
    /// موجودیت سفارش
    /// </summary>
    public class Order : BaseEntity
    {
        /// <summary>
        /// نام کاربری خریدار
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// مبلغ نهایی سفارش
        /// </summary>
        public decimal? TotalPrice { get; set; }

        /// <summary>
        /// نام مشتری
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// نام خانوادگی مشتری
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// آدرس ایمیل مشتری
        /// </summary>
        public string? EmailAddress { get; set; }

        /// <summary>
        /// آدرس کامل ارسال
        /// </summary>
        public string? AddressLine { get; set; }

        /// <summary>
        /// استان یا ایالت
        /// </summary>
        public string? State { get; set; }

        /// <summary>
        /// روش پرداخت سفارش
        /// </summary>
        public PaymentMethodEnum PaymentMethod { get; set; }
    }
}
