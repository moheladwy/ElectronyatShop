using System.ComponentModel.DataAnnotations;
using ElectronyatShop.Enums;

namespace ElectronyatShop.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public ProductType Type { get; set; }

        [Required(ErrorMessage = "Name is Required!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Image is Required!")]
        public string Image { get; set; }

        [Required(ErrorMessage = "Description is Required!")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is Required!")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Available Quantity is Required!")]
        [Range(0, int.MaxValue)]
        public int AvailableQuantity { get; set; }

        [Required(ErrorMessage = "Discount Percentage is Required!")]
		[Range(0, 100)]
		public int DiscountPercentage { get; set; } = 0;

        public decimal ActualPrice => (DiscountPercentage > 0) ? 
            (Price - (Price * DiscountPercentage / 100)): Price;

        [Required]
        public bool Status {  get; set; }

        public ICollection<CartItem>? CartItems { get; set; }

        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}
