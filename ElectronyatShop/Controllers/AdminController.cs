using ElectronyatShop.Data;
using ElectronyatShop.Models;
using ElectronyatShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ElectronyatShop.Controllers;

[Authorize("AdminRole")]
public class AdminController : Controller
{
	#region Controller Constructor and Attributes

	private SqliteDbContext Context { get; set; }

	public AdminController(SqliteDbContext context) => Context = context;

	#endregion

	#region Controller Actions

	[HttpGet]
	public async Task<IActionResult> Index() => View("Index", await Context.Products.AsNoTracking().ToListAsync());

	[HttpGet]
	public IActionResult AddNewProduct()
	{
		ProductViewModel productViewModel = new();
		FillSelectedListForProductStatus(productViewModel);
		return View("New", productViewModel);
	}

	[HttpPost]
	public async Task<IActionResult> AddNewProduct([FromForm] ProductViewModel productViewModel)
	{
		if (!ModelState.IsValid || productViewModel.Image is null || productViewModel.Image.Length == 0)
		{
			FillSelectedListForProductStatus(productViewModel);
			return View("New", productViewModel);
		}
		var product = new Product
		{
			Name = productViewModel.Name,
			Image = ProcessUploadedImage(productViewModel),
			Type = productViewModel.ProductType,
			Description = productViewModel.Description,
			Price = productViewModel.Price,
			AvailableQuantity = productViewModel.AvailableQuantity,
			DiscountPercentage = productViewModel.DiscountPercentage,
			Status = productViewModel.Status
		};
		await Context.Products.AddAsync(product);
		await Context.SaveChangesAsync();
		return RedirectToAction("Index");
	}

	[HttpGet]
	public async Task<IActionResult> EditProduct([FromRoute] int id)
	{
		var product = await Context.Products.FindAsync(id);
		if (product is null)
			return RedirectToAction("Index");

		var productViewModel = new ProductViewModel
		{
			Id = product.Id,
			Name = product.Name,
			ImageName = product.Image,
			ProductType = product.Type,
			Description = product.Description,
			Price = product.Price,
			AvailableQuantity = product.AvailableQuantity,
			DiscountPercentage = product.DiscountPercentage,
			Status = product.Status
		};
		FillSelectedListForProductStatus(productViewModel);
		return View("Edit", productViewModel);
	}

	[HttpPost]
	public async Task<IActionResult> EditProduct([FromForm] ProductViewModel productViewModel)
	{
		var product = await Context.Products.FindAsync(productViewModel.Id);
		if (!ModelState.IsValid || product is null || product.Id != productViewModel.Id)
		{
			FillSelectedListForProductStatus(productViewModel);
			return View("Edit", productViewModel);
		}
		product.Name = productViewModel.Name;
		product.Type = productViewModel.ProductType;
		product.Description = productViewModel.Description;
		product.Price = productViewModel.Price;
		product.AvailableQuantity = productViewModel.AvailableQuantity;
		product.DiscountPercentage = productViewModel.DiscountPercentage;
		product.Status = productViewModel.Status;
		if (productViewModel.Image is not null)
			product.Image = ProcessUploadedImage(productViewModel);

		Context.Products.Update(product);
		await Context.SaveChangesAsync();
		return RedirectToAction("Index");
	}

	[HttpGet]
	public async Task<IActionResult> DeleteProduct(int id)
	{
		var product = await Context.Products.FindAsync(id);
		if (product is not null)
		{
			DeleteImage(product.Image);
			Context.Products.Remove(product);
			await Context.SaveChangesAsync();
		}
		return RedirectToAction("Index");
	}

	#endregion

	#region Controller Logic

	/// <summary>
	/// Save the chosen Image on creating to the Image root
	/// </summary>
	/// <param name="productViewModel">ProductViewModel</param>
	/// <returns>Image Name</returns>
	private static string ProcessUploadedImage(ProductViewModel productViewModel)
	{
		var imagesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
		var imageName = $"{DateTime.Now:dddd-MMM-dd-yyyy-hh-mm-ss}-{productViewModel.Image?.FileName}";
		var imagePath = Path.Combine(imagesDirectory, imageName);

		var stream = new FileStream(imagePath, FileMode.Create);
		productViewModel.Image?.CopyTo(stream);

		return imageName;
	}

	private static void DeleteImage(string imageName)
	{
		var imagesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
		var imagePath = Path.Combine(imagesDirectory, imageName);
		// TODO: Figure out what the cause of the IOException that happens upon deleting the Image.
		if (System.IO.File.Exists(imagePath))
			System.IO.File.Delete(imagePath);
	}

	private static void FillSelectedListForProductStatus(ProductViewModel productViewModel)
	{
		List<SelectListItem> items =
		[
			new() { Value = "True", Text = "Yes" },
			new() { Value = "False", Text = "No" }
		];
		productViewModel.StatusList = new SelectList(items, "Value", "Text");
	}

	#endregion
}