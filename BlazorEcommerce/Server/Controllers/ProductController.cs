﻿using Microsoft.AspNetCore.Mvc;

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
        
        [HttpGet("search/{searchText}")]
        public async Task<ActionResult<ServiceResponse<Product>>> SearchProdcuts(string searchText) {
            
            var response = await _productService.SearchProducts(searchText);
            
            return Ok(response);
        }
        
        [HttpGet("searchsuggestion/{searchText}")]
        public async Task<ActionResult<ServiceResponse<Product>>> SearchProdcutsSuggestions(string searchText) {
            
            var response = await _productService.GetProductSearchSuggestions(searchText);
            
            return Ok(response);
        }


    }
}
