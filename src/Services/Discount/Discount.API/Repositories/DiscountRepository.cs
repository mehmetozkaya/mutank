using Dapper;
using Discount.API.Entities;
using Discount.API.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Threading.Tasks;

namespace Basket.API.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }       

        public async Task<Coupon> GetDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            
            var coupon = await connection.QueryFirstAsync<Coupon>
                ("SELECT * FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });
            return coupon;
        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected =
                await connection.ExecuteAsync
                    ("INSERT INTO Coupon (ProductName, Description, Value) VALUES (@ProductName, @Description, @Value)",
                            new { ProductName = coupon.ProductName, Description = coupon.Description, Value = coupon.Value });

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync
                    ("UPDATE Coupon SET ProductName=@ProductName, Description = @Description, Value = @Value WHERE Id = @Id",
                            new { ProductName = coupon.ProductName, Description = coupon.Description, Value = coupon.Value, Id = coupon.Id });

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync("DELETE FROM Coupon WHERE ProductName = @ProductName",
                new { ProductName = productName });

            if (affected == 0)
                return false;

            return true;
        }
    }
}
