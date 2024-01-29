using ElectronyatShop.DTOs;
using ElectronyatShop.Data;
using ElectronyatShop.Helpers;
using ElectronyatShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronyatShop.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize("AdminRole")]
public class AdminController : ControllerBase
{
    #region Controller Constructor and Attributes

    private ApplicationDbContext Context { get; set; }

    public AdminController(ApplicationDbContext context) => Context = context;

    #endregion
    
    #region Controller Endpoints
    
    [HttpPost]
    [Route("add-new-product")]
    public IActionResult AddNewProduct([FromForm] ProductDto productDto)
    {
        var product = AdminHelper.CreateProduct(productDto);
        Context.Products.Add(product);
        Context.SaveChanges();
        return Created();
    }
    
    [HttpPut]
    [Route("update-product")]
    public IActionResult UpdateProduct([FromForm] Product productToBeUpdated)
    {
        var product = Context.Products.Find(productToBeUpdated.Id);
        if (product == null)
            return NotFound($"Product with id = {productToBeUpdated.Id} Not Found");
        AdminHelper.UpdateProduct(product, productToBeUpdated);
        Context.Products.Update(product);
        Context.SaveChanges();
        return Ok();
    }
    
    [HttpDelete]
    [Route("delete-product/{id}")]
    public IActionResult DeleteProduct([FromRoute] int id)
    {
        var product = Context.Products.Find(id);
        if (product == null)
            return NotFound($"Product with id = {id} Not Found");
        Context.Products.Remove(product);
        Context.SaveChanges();
        return Ok();
    }
    
    #endregion
}

#region Old MVC Controller

// using ElectronyatShop.Data;
// using ElectronyatShop.Models;
// using ElectronyatShop.ViewModels;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.Rendering;
//
// namespace ElectronyatShop.Controllers
// {
//     [Authorize("AdminRole")]
//     public class AdminController : Controller
//     {
// 		#region Controller Constructor and Attributes
//
// 		private ApplicationDbContext Context { get; set; }
//
//         public AdminController(ApplicationDbContext context) => Context = context;
//
// 		#endregion
//
// 		#region Controller Actions
//
// 		[HttpGet]
//         public IActionResult Index() => View("Index", Context.Products.ToList());
//
// 		[HttpGet]
//         public IActionResult AddNewProduct()
//         {
//             ProductViewModel productViewModel = new();
//             FillSelectedListForProductStatus(productViewModel);
//             return View("New", productViewModel);
// 		}
//
//         [HttpPost]
//         public IActionResult AddNewProduct([FromForm] ProductViewModel productViewModel)
//         {
//             if (ModelState.IsValid)
//             {
//                 if (productViewModel.Image?.Length > 0)
//                 {
//                     Product product = new()
//                     {
//                         Name = productViewModel.Name,
//                         Image = ProccessUploadedImage(productViewModel),
// 						Type = productViewModel.ProductType,
//                         Description = productViewModel.Description,
//                         Price = productViewModel.Price,
//                         AvailableQuantity = productViewModel.AvailableQuantity,
//                         DiscountPercentage = productViewModel.DiscountPercentage,
//                         Status = productViewModel.Status
//                     };
// 				    Context.Products.Add(product);
// 				    Context.SaveChanges();
// 				    return RedirectToAction("Index");
//                 }
//             }
//             FillSelectedListForProductStatus(productViewModel);
//             return View("New", productViewModel);
//         }
//
//         [HttpGet]
//         public IActionResult EditProduct([FromRoute] int id)
//         {
//             Product? product = Context.Products.Find(id);
//             if (product == null)
//                 return RedirectToAction("Index");
//
//             ProductViewModel productViewModel = new()
//             {
//                 Id = product.Id,
//                 Name = product.Name,
//                 ImageName = product.Image,
//                 ProductType = product.Type,
//                 Description = product.Description,
//                 Price = product.Price,
//                 AvailableQuantity = product.AvailableQuantity,
//                 DiscountPercentage = product.DiscountPercentage,
//                 Status = product.Status
//             };
//             FillSelectedListForProductStatus(productViewModel);
//             return View("Edit", productViewModel);
//         }
//
//         [HttpPost]
//         public IActionResult EditProduct([FromForm] ProductViewModel productViewModel)
//         {
//             Product? product = Context.Products.Find(productViewModel.Id);
//             if (ModelState.IsValid && product != null && product.Id == productViewModel.Id)
//             {
//                 product.Name = productViewModel.Name;
//                 product.Type = productViewModel.ProductType;
//                 product.Description = productViewModel.Description;
//                 product.Price = productViewModel.Price;
//                 product.AvailableQuantity = productViewModel.AvailableQuantity;
//                 product.DiscountPercentage = productViewModel.DiscountPercentage;
//                 product.Status = productViewModel.Status;
//                 if (productViewModel.Image != null)
//                     product.Image = ProccessUploadedImage(productViewModel);
//
// 				Context.Products.Update(product);
//                 Context.SaveChanges();
//                 return RedirectToAction("Index");
//             }
//             FillSelectedListForProductStatus(productViewModel);
//             return View("Edit", productViewModel);
//         }
//
//         [HttpGet]
//         public IActionResult DeleteProduct(int id)
//         {
//             Product? product = Context.Products.Find(id);
//             if (product != null)
//             {
//                 DeleteImage(product.Image);
//
// 				Context.Products.Remove(product);
// 				Context.SaveChanges();
// 			}
//             return RedirectToAction("Index");
//         }
//
//         #endregion
//
// 		#region Controller Logic
//
// 		/// <summary>
// 		/// Save the chosen Image on creating to the Image root
// 		/// </summary>
// 		/// <param name="productViewModel">ProductViewModel</param>
// 		/// <returns>Image Name</returns>
// 		private string ProccessUploadedImage(ProductViewModel productViewModel)
// 		{
// 			string ImagesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
// 			string ImageName = $"{DateTime.Now:dddd-MMM-dd-yyyy-hh-mm-ss}-{productViewModel.Image?.FileName}";
// 			string ImagePath = Path.Combine(ImagesDirectory, ImageName);
//
// 			var stream = new FileStream(ImagePath, FileMode.Create);
// 			productViewModel.Image?.CopyTo(stream);
//
// 			return ImageName;
// 		}
//
//         private void DeleteImage(string ImageName)
//         {
// 			string ImagesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
// 			string ImagePath = Path.Combine(ImagesDirectory, ImageName);
//             // TODO: Figure out what the cause of the IOException that happens upon deleting the Image.
//             // System.IO.File.Delete(ImagePath);
// 		}
//
// 		private void FillSelectedListForProductStatus(ProductViewModel productViewModel)
// 		{
// 			List<SelectListItem> items = new()
// 			{
// 				new SelectListItem() { Value = "True", Text = "Yes" },
// 				new SelectListItem() { Value = "False", Text = "No" }
// 			};
// 			productViewModel.StatusList = new(items, "Value", "Text");
// 		}
//
// 		#endregion
// 	}
// }

#endregion