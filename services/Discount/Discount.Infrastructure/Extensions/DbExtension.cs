using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Discount.Infrastructure.Extensions
{
    /// <summary>
    /// Extension methods for database migration and seeding during host startup.
    /// </summary>
    public static class DatabaseMigrationExtensions
    {
        private const int MaxRetryCount = 4;
        private const int RetryDelayMilliseconds = 1000;

        /// <summary>
        /// Migrates the database and seeds initial data.
        /// </summary>
        /// <typeparam name="TContext">Type used for logging context.</typeparam>
        /// <param name="host">The host instance.</param>
        /// <returns>The same host instance for chaining.</returns>
        public static IHost MigrateDatabase<TContext>(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var logger = serviceProvider.GetRequiredService<ILogger<TContext>>();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            var contextName = typeof(TContext).Name;

            try
            {
                logger.LogInformation("Starting database migration for {ContextName}", contextName);

                ApplyMigration(configuration, logger);

                logger.LogInformation("Database migration completed successfully for {ContextName}", contextName);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during database migration for {ContextName}", contextName);
                throw;
            }

            return host;
        }

        /// <summary>
        /// Performs the actual migration: drops existing table, creates a new one, and seeds sample data.
        /// </summary>
        /// <param name="configuration">Configuration to retrieve connection string.</param>
        /// <param name="logger">Logger for diagnostic messages.</param>
        private static void ApplyMigration(IConfiguration configuration, ILogger logger)
        {
            var connectionString = configuration.GetValue<string>("DatabaseSettings:ConnectionString")
                ?? throw new InvalidOperationException("DatabaseSettings:ConnectionString is missing in configuration.");

            var retryCount = MaxRetryCount;

            while (retryCount > 0)
            {
                try
                {
                    using var connection = new NpgsqlConnection(connectionString);
                    connection.Open();

                    // 1. Drop existing table if it exists
                    using (var dropCommand = new NpgsqlCommand("DROP TABLE IF EXISTS Coupon;", connection))
                    {
                        dropCommand.ExecuteNonQuery();
                        logger.LogInformation("Dropped existing 'Coupon' table.");
                    }

                    // 2. Create the table
                    using (var createCommand = new NpgsqlCommand(@"
                        CREATE TABLE Coupon (
                            Id            SERIAL PRIMARY KEY,
                            ProductId     TEXT NOT NULL,
                            ProductName   VARCHAR(500) NOT NULL,
                            Description   TEXT,
                            Amount        INT NOT NULL
                        );", connection))
                    {
                        createCommand.ExecuteNonQuery();
                        logger.LogInformation("Created 'Coupon' table.");
                    }

                    // 3. Seed sample data
                    var sampleCoupons = GetSampleCoupons();
                    using (var insertCommand = new NpgsqlCommand())
                    {
                        insertCommand.Connection = connection;
                        insertCommand.CommandText = @"
                            INSERT INTO Coupon (ProductId, ProductName, Description, Amount)
                            VALUES (@ProductId, @ProductName, @Description, @Amount);";

                        // Prepare parameters (reused for each row)
                        var productIdParam = insertCommand.Parameters.Add("@ProductId", NpgsqlTypes.NpgsqlDbType.Text);
                        var productNameParam = insertCommand.Parameters.Add("@ProductName", NpgsqlTypes.NpgsqlDbType.Varchar, 500);
                        var descriptionParam = insertCommand.Parameters.Add("@Description", NpgsqlTypes.NpgsqlDbType.Text);
                        var amountParam = insertCommand.Parameters.Add("@Amount", NpgsqlTypes.NpgsqlDbType.Integer);

                        foreach (var coupon in sampleCoupons)
                        {
                            productIdParam.Value = coupon.ProductId;
                            productNameParam.Value = coupon.ProductName;
                            descriptionParam.Value = coupon.Description;
                            amountParam.Value = coupon.Amount;
                            insertCommand.ExecuteNonQuery();
                        }
                    }

                    logger.LogInformation("Seeded {Count} sample coupons.", sampleCoupons.Count);
                    break; // Success, exit retry loop
                }
                catch (Exception ex)
                {
                    retryCount--;
                    if (retryCount == 0)
                    {
                        logger.LogError(ex, "Migration failed after all retries.");
                        throw;
                    }

                    logger.LogWarning(ex, "Migration attempt failed. {RetriesLeft} retries left. Retrying in {Delay}ms...",
                        retryCount, RetryDelayMilliseconds);
                    Thread.Sleep(RetryDelayMilliseconds);
                }
            }
        }

        /// <summary>
        /// Provides a list of sample coupons to seed the database.
        /// </summary>
        /// <summary>
        /// Provides a list of sample coupons to seed the database.
        /// Data is selected from the product catalog with rounded prices as discount amounts.
        /// </summary>
        private static List<CouponSeedDto> GetSampleCoupons()
        {
            // Four products from different categories for variety
            return new List<CouponSeedDto>
    {
        new(
            ProductId: "66b6a1010000000000000001",
            ProductName: "Essence Mascara Lash Princess",
            Description: "Volumizing and lengthening mascara with cruelty-free formula",
            Amount: (int)Math.Round(9.99m)
        ),
        new(
            ProductId: "66b6a1010000000000000004",
            ProductName: "Red Lipstick",
            Description: "Classic bold red lipstick with creamy formula",
            Amount: (int)Math.Round(12.99m)
        ),
        new(
            ProductId: "66b6a101000000000000000a",
            ProductName: "Gucci Bloom Eau de",
            Description: "Floral fragrance with tuberose and jasmine notes",
            Amount: (int)Math.Round(79.99m) 
        ),
        new(
            ProductId: "66b6a1010000000000000010",
            ProductName: "Apple",
            Description: "Fresh and crisp apples for snacking",
            Amount: (int)Math.Round(1.99m)
        )
    };
        }

        /// <summary>
        /// Internal DTO for seeding coupons.
        /// </summary>
        private sealed record CouponSeedDto(string ProductId, string ProductName, string Description, int Amount);
    }
}