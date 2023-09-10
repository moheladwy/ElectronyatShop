using System.ComponentModel.DataAnnotations;

namespace ElectronyatShop.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        
        // TODO: Finish the ProductType Enum Assign and helpers.
        public ProductType? Type { get; set; }

        [Required(ErrorMessage = "Name is Required!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Image is Required!")]
        public string Image { get; set; }

        [Required(ErrorMessage = "Description is Required!")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is Required!")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Available Quantity is Required!")]
        public int AvailableQuantity { get; set; }

        [Required(ErrorMessage = "Discount Percentage is Required!")]
        public int DiscountPercentage { get; set; } = 0;

        [Required]
        public bool Status {  get; set; }

        public ICollection<CartItem>? CartItems { get; set; }
    }
}
