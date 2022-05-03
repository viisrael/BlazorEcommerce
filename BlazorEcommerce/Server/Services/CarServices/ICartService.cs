namespace BlazorEcommerce.Server.Services.CarServices
{
    public interface ICartService
    {
        Task<ServiceResponse<int>> GetCartItemsCount();
        Task<ServiceResponse<List<CartProductResponse>>> GetCartProductAsync(List<CartItem> cartItems);
        Task<ServiceResponse<List<CartProductResponse>>> StoreCartItems(List<CartItem> cartItems);
    }
}
