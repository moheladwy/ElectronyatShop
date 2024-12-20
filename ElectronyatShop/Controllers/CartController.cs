using ElectronyatShop.Data;
using ElectronyatShop.Models;
using ElectronyatShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectronyatShop.Controllers;

[Authorize("CustomerRole")]
public class CartController(UserManager<ApplicationUser> userManager, SqliteDbContext context)
    : Controller
{
    #region Controller Constructor and Attributes

    private string? UserId { get; set; }

    private Cart? Cart { get; set; }

    #endregion

    #region Controller Actions

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        SetCart();
        CartViewModel cartViewModel = new()
        {
            Id = Cart?.Id ?? throw new NullReferenceException("Cart is not initialized"),
            SubTotalPrice = 0,
            CartItems = Cart.CartItems?.ToList()
        };
        foreach (var item in Cart?.CartItems ?? [])
        {
            var product = await context.Products.FindAsync(item.ProductId);
            if (product is not null)
                cartViewModel.SubTotalPrice += product.ActualPrice * item.Quantity;
        }
        return View("Index", cartViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart([FromForm] CartItemViewModel cartItem)
    {
        if (!ModelState.IsValid)
            return RedirectToAction(actionName: "Index", controllerName: "Product");

        SetCart();
        var item = Cart?.CartItems?.FirstOrDefault(item => item.ProductId == cartItem.ProductId) ??
                   new CartItem { CartId = Cart?.Id, ProductId = cartItem.ProductId, Quantity = 0};

        item.Quantity += cartItem.Quantity;
        var product = await context.Products.FindAsync(item.ProductId);
        if (product is not null)
        {
            product.AvailableQuantity -= item.Quantity;
            context.Products.Update(product);
        }
        context.CartItems.Update(item);
        await context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> RemoveFromCart([FromForm]int id)
    {
        if (!ModelState.IsValid) return RedirectToAction("Index");

        SetCart();
        var item = await context.CartItems.FindAsync(id);
        if (item is null) return RedirectToAction("Index");

        var product = await context.Products.FindAsync(item.ProductId);
        if (product is not null )
        {
            product.AvailableQuantity += item.Quantity;
            context.Products.Update(product);
        }
        Cart?.CartItems?.Remove(item);
        context.CartItems.Remove(item);
        await context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    #endregion

    #region Controller Logic

    private void SetCart()
    {
        UserId = userManager.GetUserId(User);
        Cart = new Cart();
        if (UserId is not null)
            Cart = context.Carts.Include(userCart => userCart.CartItems).First(c => c.UserId == UserId);
    }

    #endregion
}