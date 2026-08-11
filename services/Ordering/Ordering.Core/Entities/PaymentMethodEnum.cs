namespace Ordering.Core.Entities
{
    /// <summary>
    /// روش‌های پرداخت پشتیبانی‌شده در میکروسرویس سفارشات
    /// </summary>
    public enum PaymentMethodEnum
    {
        /// <summary>
        /// پرداخت نقدی یا پرداخت در محل (Cash on Delivery)
        /// </summary>
        Cash = 1,

        /// <summary>
        /// پرداخت از طریق کارت اعتباری
        /// </summary>
        CreditCard = 2,

        /// <summary>
        /// انتقال بانکی مستقیم (کارت به کارت یا حواله پایا/ساتنا)
        /// </summary>
        BankTransfer = 3,

        /// <summary>
        /// درگاه بین‌المللی PayPal
        /// </summary>
        PayPal = 4,

        /// <summary>
        /// درگاه پرداخت آنلاین بانکی (شتابی)
        /// </summary>
        OnlineGateway = 5,

        /// <summary>
        /// پرداخت با رمزارزها (ارزهای دیجیتال)
        /// </summary>
        Crypto = 6
    }
}
