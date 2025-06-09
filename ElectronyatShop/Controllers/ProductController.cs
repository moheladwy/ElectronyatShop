using ElectronyatShop.Data;
using ElectronyatShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectronyatShop.Controllers;

[Authorize("CustomerRole")]
public class ProductController : Controller
{
    #region Controller Constructor and Attributes

    public const int ProductNotFound = -1;

    private readonly ElectronyatShopDbContext _context;

    public ProductController(ElectronyatShopDbContext context) => _context = context;

    #endregion

    #region Controller Actions

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (IsUserAuthenticated() && IsUserAdmin())
            return RedirectToAction(actionName: "Index", controllerName: "Admin");

        return View("Index", await _context.Products.Where(p => p.Status == true).AsNoTracking().ToListAsync());
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Details([FromRoute] int id)
    {
        var productItem = await _context.Products.FindAsync(id);
        if (productItem is null)
            return RedirectToAction("Index");
        return View("Details", new CartItemViewModel { ProductId = productItem.Id });
    }

    #endregion

    #region Helper Methods

    private bool IsUserAuthenticated() => User.Identity?.IsAuthenticated ?? false;

    private bool IsUserAdmin() => User.HasClaim("Admin", "Admin");

    #endregion
}