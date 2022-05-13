namespace BlazorEcommerce.Server.Services.CarServices
{
    public interface ICartService
    {
        Task<ServiceResponse<int>> GetCartItemsCount();
        Task<ServiceResponse<bool>> AddToCart(CartItem cartItem);
        Task<ServiceResponse<bool>> UpdateQuantity(CartItem cartItem);
        Task<ServiceResponse<List<CartProductResponse>>> GetDbCartProducts();
        Task<ServiceResponse<List<CartProductResponse>>> StoreCartItems(List<CartItem> cartItems);
        Task<ServiceResponse<List<CartProductResponse>>> GetCartProductAsync(List<CartItem> cartItems);
    }
}
