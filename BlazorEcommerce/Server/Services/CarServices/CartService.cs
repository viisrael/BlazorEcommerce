using System.Security.Claims;

namespace BlazorEcommerce.Server.Services.CarServices
{
    public class CartService : ICartService
    {
        private readonly DataContext _context;
        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(DataContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<ServiceResponse<List<CartProductResponse>>> GetCartProductAsync(List<CartItem> cartItems)
        {
            var result = new ServiceResponse<List<CartProductResponse>>()
            {
                Data = new List<CartProductResponse>()
            };

            foreach (var item in cartItems)
            {
                var product = await _context.Products.Where(p => p.Id.Equals(item.ProductId)).FirstOrDefaultAsync();

                if (product == null)
                {
                    continue;
                }

                var productVariant = await _context.ProductVariants.Where(v => v.ProductTypeId.Equals(item.ProductTypeId) && v.ProductId.Equals(item.ProductId))
                                                                   .Include(v => v.ProductType)
                                                                   .FirstOrDefaultAsync();

                if (productVariant == null)
                    continue;

                var cartProduct = new CartProductResponse
                {
                    ProductId = product.Id,
                    ImageUrl = product.ImageUrl,
                    Title = product.Title,
                    Price = productVariant.Price,
                    ProductType = productVariant.ProductType.Name,
                    ProductTypeId = productVariant.ProductTypeId,
                    Quantity = item.Quantity,
                };

                result.Data.Add(cartProduct);
            }

            return result;
        }

        public async Task<ServiceResponse<List<CartProductResponse>>> StoreCartItems(List<CartItem> cartItems)
        {
            cartItems.ForEach(cartItem => cartItem.UserId = _authService.GetUserId());

            _context.CartItems.AddRange(cartItems);
            await _context.SaveChangesAsync();

            return await GetDbCartProducts();
        }

        public async Task<ServiceResponse<int>> GetCartItemsCount()
        {
            var count = (await _context.CartItems.Where(ci => ci.UserId.Equals(_authService.GetUserId())).ToListAsync()).Count;

            return new ServiceResponse<int>
            {
                Data = count,
            };
        }

        public async Task<ServiceResponse<List<CartProductResponse>>> GetDbCartProducts(int? userId = null)
        {
            if(userId == null)
                userId = _authService.GetUserId();

            return await GetCartProductAsync(await _context.CartItems.Where(ci => ci.UserId.Equals(userId)).ToListAsync());
        }

        public async Task<ServiceResponse<bool>> AddToCart(CartItem cartItem)
        {
            cartItem.UserId = _authService.GetUserId();

            var sameItem = await _context.CartItems.FirstOrDefaultAsync(ci => ci.ProductId == cartItem.ProductId &&
                                                                              ci.ProductTypeId == cartItem.ProductTypeId &&
                                                                              ci.UserId == cartItem.UserId);

            if (sameItem == null)
            {
                _context.CartItems.Add(cartItem);
            }
            else
            {
                sameItem.Quantity = cartItem.Quantity;
            }

            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }

        public async Task<ServiceResponse<bool>> UpdateQuantity(CartItem cartItem)
        {
            var dbCartItem = await _context.CartItems.FirstOrDefaultAsync(ci => ci.ProductId == cartItem.ProductId &&
                                                                              ci.ProductTypeId == cartItem.ProductTypeId &&
                                                                              ci.UserId == _authService.GetUserId());

            if(dbCartItem == null)
                return new ServiceResponse<bool> { Data= false, Success=false,Message= "Cart item does not exists." };

            dbCartItem.Quantity = cartItem.Quantity;
            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data= true };

        }

        public async Task<ServiceResponse<bool>> RemoveItemFromCart(int productId, int productTypeId)
        {
            var dbCartItem = await _context.CartItems.FirstOrDefaultAsync(ci => ci.ProductId == productId &&
                                                                                ci.ProductTypeId == productTypeId &&
                                                                                ci.UserId == _authService.GetUserId());

            if (dbCartItem == null)
                return new ServiceResponse<bool> { Data = false, Success = false, Message = "Cart item does not exists." };

            _context.CartItems.Remove(dbCartItem);
            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }
    }
}
