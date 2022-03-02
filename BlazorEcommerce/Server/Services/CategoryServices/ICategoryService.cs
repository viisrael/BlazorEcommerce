namespace BlazorEcommerce.Server.Services.CategoryServices
{
    public interface ICategoryService
    {
        Task<ServiceResponse<Category>> GetCategoryAsync(int id);
        Task<ServiceResponse<List<Category>>> GetCategoriesAsync();
    }
}
