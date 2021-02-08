namespace Discount.API.Entities
{
    public class Coupon
    {
        public int Id { get; set; }
        public string ProductId { get; set; }
        public string Name { get; set; }
        public int Value { get; set; }
    }
}
