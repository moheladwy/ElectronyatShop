using ElectronyatShopWebAPI.Data;
using ElectronyatShopWebAPI.DTOs;
using ElectronyatShopWebAPI.Helpers;
using ElectronyatShopWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronyatShopWebAPI.Controllers;

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