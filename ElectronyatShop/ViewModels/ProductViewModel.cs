using ElectronyatShop.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ElectronyatShop.ViewModels
{
    [Authorize("AdminRole")]
	public class ProductViewModel
	{
		[Key]
		public int Id { get; set; }

		[Required(ErrorMessage = "Name is Required!")]
		public string Name { get; set; }

		public string? ImageName { get; set; }

		[Required]
		public ProductType ProductType { get; set; }

		public IFormFile? Image { get; set; }

		[Required(ErrorMessage = "Description is Required!")]
		public string Description { get; set; }

		[Required(ErrorMessage = "Price is Required!")]
		[Range(0, int.MaxValue, ErrorMessage = $"Price cannot be less than 0!")]
		public decimal Price { get; set; }

		[Required(ErrorMessage = "Available Quantity is Required!")]
		[Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be less than 0!")]
		public int AvailableQuantity { get; set; }

		[Required(ErrorMessage = "Discount Percentage is Required!")]
		[Range(0, 100, ErrorMessage = "Discount Percentage cannot be less than 0% or more than 100%!")]
		public int DiscountPercentage { get; set; } = 0;

		public SelectList? StatusList { get; set; }

		[Required]
		public bool Status {  get; set; }
	}
}
