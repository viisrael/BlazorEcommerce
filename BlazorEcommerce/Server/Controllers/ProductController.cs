using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }


        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProducts() {
            var response = await _productService.GetProductsAsync();
            
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<Product>>> GetProduct(int id) {
            var response = await _productService.GetProductAsync(id);
            
            return Ok(response);
        }
        
        [HttpGet("category/{categoryUrl}")]
        public async Task<ActionResult<ServiceResponse<Product>>> GetProductByCategory(string categoryUrl) {
            var response = await _productService.GetProductsByCategoryAsync(categoryUrl);
            
            return Ok(response);
        }
        
        [HttpGet("search/{searchText}/{page}")]
        public async Task<ActionResult<ServiceResponse<ProductSearchResultDTO>>> SearchProducts(string searchText, int page= 1) {
            
            var response = await _productService.SearchProducts(searchText, page);
            
            return Ok(response);
        }
        
        [HttpGet("searchsuggestions/{searchText}")]
        public async Task<ActionResult<ServiceResponse<Product>>> SearchProductsSuggestions(string searchText) {
            
            var response = await _productService.GetProductSearchSuggestions(searchText);
            
            return Ok(response);
        }
        
        [HttpGet("featured")]
        public async Task<ActionResult<ServiceResponse<Product>>> GetFeaturedProducts() {

            var response = await _productService.GetFeaturedProduct();
            
            return Ok(response);
        }


    }
}
