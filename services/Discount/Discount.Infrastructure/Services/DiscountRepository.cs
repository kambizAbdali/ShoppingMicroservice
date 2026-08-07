using Dapper;
using Discount.Core.Entities;
using Discount.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Discount.Infrastructure.Services
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly string _connectionString;

        public DiscountRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>("DatabaseSettings:ConnectionString")
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string is missing.");
        }

        public async Task<Coupon> GetDiscountByProductIdAsync(string productId)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            const string sql = "SELECT * FROM Coupon WHERE ProductId = @ProductId";
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(sql, new { ProductId = productId });
            return coupon ?? new Coupon();
        }

        public async Task<Coupon> GetDiscountByProductNameAsync(string productName)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            const string sql = "SELECT * FROM Coupon WHERE ProductName = @ProductName";
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(sql, new { ProductName = productName });
            return coupon ?? new Coupon();
        }

        public async Task<bool> CreateDiscountAsync(Coupon coupon)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            const string sql = @"
                INSERT INTO Coupon (ProductId, ProductName, Description, Amount)
                VALUES (@ProductId, @ProductName, @Description, @Amount)";
            var affectedRows = await connection.ExecuteAsync(sql, coupon);
            return affectedRows > 0;
        }

        public async Task<bool> UpdateDiscountAsync(Coupon coupon)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            const string sql = @"
                UPDATE Coupon
                SET ProductId = @ProductId,
                    ProductName = @ProductName,
                    Description = @Description,
                    Amount = @Amount
                WHERE Id = @Id";
            var affectedRows = await connection.ExecuteAsync(sql, coupon);
            return affectedRows > 0;
        }

        public async Task<bool> DeleteDiscountByProductIdAsync(string productId)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            const string sql = "DELETE FROM Coupon WHERE ProductId = @ProductId";
            var affectedRows = await connection.ExecuteAsync(sql, new { ProductId = productId });
            return affectedRows > 0;
        }

        public async Task<bool> DeleteDiscountByProductNameAsync(string productName)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            const string sql = "DELETE FROM Coupon WHERE ProductName = @ProductName";
            var affectedRows = await connection.ExecuteAsync(sql, new { ProductName = productName });
            return affectedRows > 0;
        }
    }
}