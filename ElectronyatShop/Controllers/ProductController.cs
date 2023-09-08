using ElectronyatShop.Data;
using ElectronyatShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronyatShop.Controllers
{
    [Authorize("CustomerRole")]
    public class ProductController : Controller
    {
        public const int PRODUCT_NOT_FOUND = -1;

        private readonly ApplicationDbContext Context;

        private readonly Cart? cart;

        public ProductController(ApplicationDbContext context)
        {
            Context = context;
            //var userId = Context.Users.Where(u => u.UserName == User.Identity.Name).First().Id;
            //cart = context.Carts.FirstOrDefault(c => c.UserId == userId);
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
            Product? product = Context.Products.Find(id);
            
            if (product == null)
                RedirectToAction("Index");
            
            return View("Details", product);
        }

        [HttpGet]
        public IActionResult AddToCart([FromRoute] int id)
        {
            Product? product = Context.Products.Find(id);
            if (product == null || product.AvailableQuantity == 0)
                return RedirectToAction("Index");
            product.AvailableQuantity--;
            //cart?.Products?.Add(product);
            return RedirectToAction("Index");
        }

        #endregion
    }
}