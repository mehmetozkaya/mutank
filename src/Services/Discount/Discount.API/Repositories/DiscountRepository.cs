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

        public async Task<Coupon> GetDiscount(string productId)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            
            var coupon = await connection.QueryFirstAsync<Coupon>
                ("SELECT * FROM Coupon WHERE ProductId = @ProductId", new { ProductId = productId });
            return coupon;
        }
    }
}
