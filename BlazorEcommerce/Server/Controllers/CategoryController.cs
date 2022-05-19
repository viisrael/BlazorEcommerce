using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Category>>>> GetCategory()
        {
            var result = await _categoryService.GetCategoriesAsync();

            return Ok(result);
        }
        
        [HttpGet("admin/category"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Category>>>> GetCategoryAdminCategories()
        {
            var result = await _categoryService.GetAdminCategoriesAsync();

            return Ok(result);
        }
        
        [HttpDelete("admin/category/{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Category>>>> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteCategoriesAsync(id);

            return Ok(result);
        }
        
        [HttpPost("admin/category"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Category>>>> AddCategory(Category category)
        {
            var result = await _categoryService.AddCategoriesAsync(category);

            return Ok(result);
        }
        
        [HttpPut("admin/category"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Category>>>> UpdateCategory(Category category)
        {
            var result = await _categoryService.UpdateCategoriesAsync(category);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<Category>>> GetCategory(int id)
        {
            var result = await _categoryService.GetCategoryAsync(id);

            return Ok(result);
        }
    }
}
