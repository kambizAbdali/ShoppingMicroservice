using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Ordering.Infrastructure.Data
{
    /// <summary>
    /// ایجاد OrderContext در زمان اجرای EF Core CLI
    /// </summary>
    public class OrderContextFactory
        : IDesignTimeDbContextFactory<OrderContext>
    {
        public OrderContext CreateDbContext(string[] args)
        {
            var environment =
                Environment.GetEnvironmentVariable(
                    "ASPNETCORE_ENVIRONMENT")
                ?? "Development";

            var basePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "../Ordering.Api");

            if (!Directory.Exists(basePath))
            {
                basePath = Directory.GetCurrentDirectory();
            }

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile(
                    "appsettings.json",
                    optional: false,
                    reloadOnChange: false)
                .AddJsonFile(
                    $"appsettings.{environment}.json",
                    optional: true,
                    reloadOnChange: false)
                .AddEnvironmentVariables()
                .Build();

            var connectionString =
                configuration.GetConnectionString("OrderingDb")
                ?? throw new InvalidOperationException(
                    "Connection string 'OrderingDb' was not found.");

            var optionsBuilder =
                new DbContextOptionsBuilder<OrderContext>();

            optionsBuilder.UseSqlServer(connectionString);

            optionsBuilder.UseAsyncSeeding(async (context, _, token) =>
            {
                await OrderSeedData.SeedDataAsync(context);
            });

            return new OrderContext(optionsBuilder.Options);
        }
    }
}
