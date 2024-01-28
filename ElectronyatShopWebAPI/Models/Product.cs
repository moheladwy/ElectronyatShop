using System.ComponentModel.DataAnnotations;
using ElectronyatShopWebAPI.Enums;

namespace ElectronyatShopWebAPI.Models;

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
    [Range(0, int.MaxValue, ErrorMessage = "Price must be greater than or equal to 0")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Available Quantity is Required!")]
    [Range(0, int.MaxValue, ErrorMessage = "Available Quantity must be greater than or equal to 0")]
    public int AvailableQuantity { get; set; }

    [Required(ErrorMessage = "Discount Percentage is Required!")]
    [Range(0, 100, ErrorMessage = "Discount Percentage must be between 0 and 100")]
    public int DiscountPercentage { get; set; } = 0;

    public decimal ActualPrice => Price - (Price * DiscountPercentage / 100);

    [Required]
    public bool Status {  get; set; }

    public ICollection<CartItem>? CartItems { get; set; }

    public ICollection<OrderItem>? OrderItems { get; set; }
}