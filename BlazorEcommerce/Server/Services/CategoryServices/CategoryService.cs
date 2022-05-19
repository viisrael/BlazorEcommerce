namespace BlazorEcommerce.Server.Services.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _context;

        public CategoryService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<Category>>> AddCategoriesAsync(Category category)
        {
            category.Editing = category.IsNew = false;

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return await GetAdminCategoriesAsync();
        }

        public async Task<ServiceResponse<List<Category>>> DeleteCategoriesAsync(int id)
        {
            Category category = (await GetCategoryAsync(id)).Data;

            if(category == null)
            {
                return new ServiceResponse<List<Category>>
                {
                    Success = false,
                    Message = "Cateogry not found"
                };
            }

            category.Deleted = true;

            await _context.SaveChangesAsync();

            return await GetAdminCategoriesAsync();
        }

        public async Task<ServiceResponse<List<Category>>> GetAdminCategoriesAsync()
        {
            var categories = await _context.Categories
                                               .Where(c => !c.Deleted)
                                               .ToListAsync();

            return new ServiceResponse<List<Category>>
            {
                Data = categories
            };
        }

        public async Task<ServiceResponse<List<Category>>> GetCategoriesAsync()
        {
            var categories = await _context.Categories
                                               .Where(c => !c.Deleted && c.Visible)
                                               .ToListAsync();

            return new ServiceResponse<List<Category>>
            {
                Data = categories
            };
        }

        public async Task<ServiceResponse<Category>> GetCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            return new ServiceResponse<Category> { Data = category };
        }

        public async Task<ServiceResponse<List<Category>>> UpdateCategoriesAsync(Category category)
        {
            var dbCategory = (await GetCategoryAsync(category.Id)).Data;
            if(category == null)
            {
                return new ServiceResponse<List<Category>>
                {
                    Success = false,
                    Message = "Cateogry not found"
                };
            }

            dbCategory.Name = category.Name;
            dbCategory.Url = category.Url;
            dbCategory.Visible = category.Visible;

            await _context.SaveChangesAsync();
            
            return await GetAdminCategoriesAsync();
        }
    }
}
