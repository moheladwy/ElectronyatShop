using ElectronyatShop.Data;
using ElectronyatShop.Models;
using ElectronyatShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronyatShop.Controllers
{
    [Authorize("CustomerRole")]
    public class ProductController : Controller
    {
        public const int PRODUCT_NOT_FOUND = -1;

        private readonly ApplicationDbContext Context;

        public ProductController(ApplicationDbContext context)
        {
            Context = context;
        }

        #region Controller Actions

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            if ((User.Identity?.IsAuthenticated ?? false) && User.HasClaim("Admin", "Admin"))
                return RedirectToAction(actionName: "Index", controllerName: "Admin");
            return View("Index", Context.Products.Where(p => p.Status == true).ToList());
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Details([FromRoute] int id)
        {
            Product? productItem = Context.Products.Find(id);
            
            if (productItem == null)
                RedirectToAction("Index");

            CartItemViewModel item = new() { ProductId = productItem?.Id };
            
            return View("Details", item);
        }

        #endregion
    }
}