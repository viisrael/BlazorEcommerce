namespace BlazorEcommerce.Server.Services.CategoryServices
{
    public interface ICategoryService
    {
        Task<ServiceResponse<Category>> GetCategoryAsync(int id);
        Task<ServiceResponse<List<Category>>> GetCategoriesAsync();
        Task<ServiceResponse<List<Category>>> GetAdminCategoriesAsync();
        Task<ServiceResponse<List<Category>>> AddCategoriesAsync(Category category);
        Task<ServiceResponse<List<Category>>> UpdateCategoriesAsync(Category category);
        Task<ServiceResponse<List<Category>>> DeleteCategoriesAsync(int id);
    }
}
