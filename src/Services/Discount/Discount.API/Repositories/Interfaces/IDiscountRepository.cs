using Discount.API.Entities;
using System.Threading.Tasks;

namespace Discount.API.Repositories.Interfaces
{
    public interface IDiscountRepository
    {
        Task<Coupon> GetDiscount(string productId);
    }
}
