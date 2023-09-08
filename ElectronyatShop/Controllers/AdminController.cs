﻿using ElectronyatShop.Data;
using ElectronyatShop.Models;
using ElectronyatShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ElectronyatShop.Controllers
{
    [Authorize("AdminRole")]
    public class AdminController : Controller
    {
		private ApplicationDbContext Context { get; set; }

        public AdminController(ApplicationDbContext context) => Context = context;
		
        #region Controller Actions

		[HttpGet]
        public IActionResult Index() => View("Index", Context.Products.ToList());

		[HttpGet]
        public IActionResult AddNewProduct()
        {
            ProductViewModel productViewModel = new();
            FillSelectedListForProductStatus(productViewModel);
            return View("New", productViewModel);
		}

        [HttpPost]
        public IActionResult AddNewProduct([FromForm] ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                if (productViewModel.Image?.Length > 0)
                {
                    Product product = new()
                    {
                        Name = productViewModel.Name,
                        Image = ProccessUploadedImage(productViewModel),
                        Description = productViewModel.Description,
                        Price = productViewModel.Price,
                        AvailableQuantity = productViewModel.AvailableQuantity,
                        DiscountPercentage = productViewModel.DiscountPercentage,
                        Status = productViewModel.Status
                    };
				    Context.Products.Add(product);
				    Context.SaveChanges();
				    return RedirectToAction("Index");
                }
            }
            FillSelectedListForProductStatus(productViewModel);
            return View("New", productViewModel);
        }

        [HttpGet]
        public IActionResult EditProduct([FromRoute] int id)
        {
            Product? product = Context.Products.Find(id);
            if (product == null)
                return RedirectToAction("Index");

            ProductViewModel productViewModel = new()
            {
                Id = product.Id,
                Name = product.Name,
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
        public IActionResult EditProduct([FromForm] ProductViewModel productViewModel)
        {
            Product? product = Context.Products.Find(productViewModel.Id);
            if (ModelState.IsValid && product != null)
            {
                product.Name = productViewModel.Name;
                product.Description = productViewModel.Description;
                product.Price = productViewModel.Price;
                product.AvailableQuantity = productViewModel.AvailableQuantity;
                product.DiscountPercentage = productViewModel.DiscountPercentage;
                product.Status = productViewModel.Status;
				// TODO: Make the Image Update. 
                // product.Image = productViewModel.Image

				Context.Products.Update(product);
                Context.SaveChanges();
                return RedirectToAction("Index");
            }
		    // TODO: Make the Image Update. 
			string ImagesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
			string ImagePath = Path.Combine(ImagesDirectory, product.Image);
            var stream = System.IO.File.OpenRead(ImagePath);
            productViewModel.Image = new FormFile(stream, 0, stream.Length, null, product.Image);
            
            FillSelectedListForProductStatus(productViewModel);
            return View("Edit", productViewModel);
        }

        [HttpGet]
        public IActionResult DeleteProduct(int id)
        {
            Product? product = Context.Products.Find(id);
            if (product != null)
            {
                DeleteImage(product.Image);

				Context.Products.Remove(product);
				Context.SaveChanges();
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
		private string ProccessUploadedImage(ProductViewModel productViewModel)
		{
			string ImagesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
			string ImageName = $"{DateTime.Now:dddd-MMM-dd-yyyy-hh-mm-ss}-{productViewModel.Image?.FileName}";
			string ImagePath = Path.Combine(ImagesDirectory, ImageName);

			var stream = new FileStream(ImagePath, FileMode.Create);
			productViewModel.Image?.CopyTo(stream);

			return ImageName;
		}

        private void DeleteImage(string ImageName)
        {
			string ImagesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
			string ImagePath = Path.Combine(ImagesDirectory, ImageName);
            // TODO: Figure out what the cause of the IOException that happens upon deleting the Image.
            // System.IO.File.Delete(ImagePath);
		}

		private void FillSelectedListForProductStatus(ProductViewModel productViewModel)
		{
			List<SelectListItem> items = new()
			{
				new SelectListItem() { Value = "True", Text = "Yes" },
				new SelectListItem() { Value = "False", Text = "No" }
			};
			productViewModel.StatusList = new(items, "Value", "Text");
		}

		#endregion
	}
}
