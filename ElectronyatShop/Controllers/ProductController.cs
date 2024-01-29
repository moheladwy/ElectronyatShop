using ElectronyatShop.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronyatShop.Controllers
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

# region  Old MVC Controller
// using ElectronyatShop.Data;
// using ElectronyatShop.Models;
// using ElectronyatShop.ViewModels;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
//
// namespace ElectronyatShop.Controllers
// {
//     [Authorize("CustomerRole")]
//     public class ProductController : Controller
//     {
// 		#region Controller Constructor and Attributes
//
//         private readonly ApplicationDbContext context;
//
//         public ProductController(ApplicationDbContext context)
//         {
//             this.context = context;
//         }
//
// 		#endregion
//
// 		#region Controller Actions
//
// 		[AllowAnonymous]
//         [HttpGet]
//         public IActionResult Index()
//         {
//             if ((User.Identity?.IsAuthenticated ?? false) && User.HasClaim("Admin", "Admin"))
//                 return RedirectToAction(actionName: "Index", controllerName: "Admin");
//             return View("Index", context.Products.Where(p => p.Status == true).ToList());
//         }
//
//         [AllowAnonymous]
//         [HttpGet]
//         public IActionResult Details([FromRoute] int id)
//         {
//             Product? productItem = context.Products.Find(id);
//             
//             if (productItem == null)
//                 RedirectToAction("Index");
//
//             CartItemViewModel item = new() { ProductId = productItem?.Id };
//             
//             return View("Details", item);
//         }
//
//         #endregion
//     }
// }
#endregion