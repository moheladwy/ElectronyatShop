using ElectronyatShop.Models;

namespace ElectronyatShop.ViewModels
{
    public class CartViewModel
    {
        public int Id { get; set; }

        public const int ShippingCost = 10;

		public decimal SubTotalPrice { get; set; } = 0;

		public decimal TotalPrice => SubTotalPrice + ShippingCost;

        public string? UserId { get; set; }

        public List<CartItem>? CartItems { get; set; }
    }
}
