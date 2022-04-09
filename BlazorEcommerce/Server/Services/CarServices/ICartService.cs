namespace BlazorEcommerce.Server.Services.CarServices
{
    public interface ICartService
    {
        Task<ServiceResponse<List<CartProductResponse>>> GetCartProductAsync(List<CartItem> cartItems);
    }
}
