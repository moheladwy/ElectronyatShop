using ElectronyatShopWebAPI.Data;
using ElectronyatShopWebAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectronyatShopWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize("CustomerRole")]
public class CartController : ControllerBase
{
    #region Controller Constructor and Attributes

    private ApplicationDbContext Context { get; set; }

    public CartController(ApplicationDbContext context)
    {
        Context = context;
    }

    #endregion
    
    #region Controller Endpoints

    [HttpGet("{userId}")]
    [Route("get/{userId}")]
    public IActionResult Get([FromRoute] string userId)
    {
        var cart = Context.Carts.Include(c => c.CartItems).First(c => c.UserId == userId);
        return Ok(CartHelper.ConvertCartToCartDto(cart, Context));
    }
    
    [HttpPost]
    [Route("add-item")]
    public IActionResult Add([FromBody] CartItemDto cartItem)
    {
        var item = CartHelper.ConvertCartItemDtoToCartItem(cartItem, Context);
        Context.CartItems.Add(item);
        Context.SaveChanges();
        return Created();
    }
    
    [HttpDelete]
    [Route("remove-item/{id}")]
    public IActionResult Remove([FromRoute] int id)
    {
        var item = Context.CartItems.Find(id);
        if (item == null)
            return NotFound();
        Context.CartItems.Remove(item);
        Context.SaveChanges();
        return Ok();
    }
    
    #endregion
}