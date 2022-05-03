using BlazorEcommerce.Server.Services.CarServices;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlazorEcommerce.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }



        [HttpPost("products")]
        public async Task<ActionResult<ServiceResponse<List<CartProductResponse>>>> GetCartProducts([FromBody] List<CartItem> cartItems)
        {
            var result = await _cartService.GetCartProductAsync(cartItems);

            return Ok(result);
        }
        
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<CartProductResponse>>>> StoreCartItems([FromBody] List<CartItem> cartItems)
        {
            var result = await _cartService.StoreCartItems(cartItems);

            return Ok(result);
        }

        [HttpGet("count")]
        public async Task<ActionResult<ServiceResponse<int>>> GetCartItemsCount()
        {
            return await _cartService.GetCartItemsCount();
        }
    }
}
