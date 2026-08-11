using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Ordering.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data
{
    public static class OrderSeedData
    {
        public static async Task SeedDataAsync(
            OrderContext context,
            ILogger? logger = null,
            CancellationToken cancellationToken = default)
        {
            // استفاده از NullLogger در صورتی که لاگری ارسال نشده باشد (مثلاً در زمان DesignTimeFactory)
            logger ??= NullLogger.Instance;

            try
            {
                // بررسی آسنکرون وجود داده جهت جلوگیری از Insert تکراری
                if (!await context.Orders.AnyAsync(cancellationToken))
                {
                    logger.LogInformation("Seeding database associated with context {DbContextName}", nameof(OrderContext));

                    await context.Orders.AddRangeAsync(GetPreconfiguredOrders(), cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);

                    logger.LogInformation("Database seeded successfully associated with context {DbContextName}", nameof(OrderContext));
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database associated with context {DbContextName}", nameof(OrderContext));
                throw;
            }
        }

        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>
            {
                new Order
                {
                    UserName = "kambiz",
                    FirstName = "Kambiz",
                    LastName = "Abdali",
                    EmailAddress = "kambiz@test.com",
                    AddressLine = "Tehran, Iran",
                    State = "Tehran",
                    TotalPrice = 350,
                    PaymentMethod = PaymentMethodEnum.OnlineGateway,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true,
                    Version = 1
                },
                new Order
                {
                    UserName = "sara",
                    FirstName = "Sara",
                    LastName = "Ahmadi",
                    EmailAddress = "sara@test.com",
                    AddressLine = "Isfahan, Iran",
                    State = "Isfahan",
                    TotalPrice = 120,
                    PaymentMethod = PaymentMethodEnum.CreditCard,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true,
                    Version = 1
                }
            };
        }
    }
}
