namespace BlazorEcommerce.Client.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _http;
        public List<Category> Categories { get; set; } = new List<Category>();

        public CategoryService(HttpClient http)
        {
            _http = http;
        }


        public async Task GetCategories()
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<List<Category>>>("api/category");

            if (response != null)
                Categories = response.Data;


        }

        public async Task<ServiceResponse<Category>> GetCategory(int id)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<Category>>($"api/category/{id}");


            return result;
        }
    }
}
