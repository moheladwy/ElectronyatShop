using ElectronyatShop.Data;
using ElectronyatShop.Models;
using ElectronyatShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectronyatShop.Controllers
{
    [Authorize("CustomerRole")]
    public class CartController : Controller
    {
        #region Controller Constructor and Attributes

        private SqliteDbContext Context { get; set; }

        private string? userId { get; set; }

        private Cart? cart { get; set; }

        private UserManager<ApplicationUser> UserManager { get; set; }

        public CartController(UserManager<ApplicationUser> userManger, SqliteDbContext context)
        {
            Context = context;
            UserManager = userManger;
        }

        #endregion

        #region Controller Actions

        [HttpGet]
        public IActionResult Index()
        {
            SetCart();
            CartViewModel cartViewModel = new()
            {
                Id = cart.Id,
                SubTotalPrice = 0,
                CartItems = cart.CartItems.ToList()
            };
            foreach (CartItem item in cart.CartItems)
            {
                Product? product = Context.Products.Find(item.ProductId);
                if (product != null)
                    cartViewModel.SubTotalPrice += (product.ActualPrice * item.Quantity);
            }
            return View("Index", cartViewModel);
        }

        [HttpPost]
        public IActionResult AddToCart([FromForm] CartItemViewModel cartItem)
        {
            if (cartItem != null && ModelState.IsValid)
            {
                SetCart();

				CartItem item = cart.CartItems.Where(item => item.ProductId == cartItem.ProductId).FirstOrDefault();
                if (item == null)
					item = new() { CartId = cart.Id, ProductId = cartItem.ProductId, Quantity = 0};
                item.Quantity += cartItem.Quantity;

                Product? product = Context.Products.Find(item.ProductId);
				if (product != null)
				{
					product.AvailableQuantity -= item.Quantity;
					Context.Products.Update(product);
				}

				Context.CartItems.Update(item);
				Context.SaveChanges();
				return RedirectToAction("Index");
            }
            return RedirectToAction(actionName: "Index", controllerName: "Product");
        }

        [HttpPost]
        public IActionResult RemoveFromCart([FromForm]int id) 
        {
            if (ModelState.IsValid)
            {
                SetCart();
                CartItem? item = Context.CartItems.Find(id);
                if (item != null)
                {
                    Product? product = Context.Products.Find(item.ProductId);
                    if (product != null )
                    {
                        product.AvailableQuantity += item.Quantity;
                        Context.Products.Update(product);
                    }
                    cart?.CartItems?.Remove(item);
                    Context.CartItems.Remove(item);
                    Context.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region Controller Logic

        private void SetCart()
        {
			userId = UserManager.GetUserId(User);
			cart = new();
			if (userId != null)
				cart = Context.Carts.Include(cart => cart.CartItems).Where(c => c.UserId == userId).First();
		}

        #endregion
    }
}
