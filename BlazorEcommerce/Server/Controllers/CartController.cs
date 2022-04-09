using BlazorEcommerce.Server.Services.CarServices;
using Microsoft.AspNetCore.Mvc;

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
    }
}
