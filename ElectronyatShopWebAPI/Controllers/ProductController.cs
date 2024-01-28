using ElectronyatShopWebAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronyatShopWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize("CustomerRole")]
    public class ProductController : ControllerBase
    {
        #region Controller Constructor and Attributes

        private readonly ApplicationDbContext context;

        public ProductController(ApplicationDbContext context)
        {
            this.context = context;
        }

        #endregion

        #region Controller Endpoints

        [AllowAnonymous]
        [HttpGet]
        [Route("get-all")]
        public IActionResult GetAllProducts()
        {
            var products = context.Products.ToList();
            return Ok(products);
        }
        
        [AllowAnonymous]
        [HttpGet("{id}")]
        [Route("get-by-id/{id}")]
        public IActionResult GetProductById([FromRoute] int id)
        {
            var product = context.Products.Find(id);
            if (product == null)
                return NotFound($"Product with id = {id} Not Found");
            return Ok(product);
        }

        #endregion 
    }
}