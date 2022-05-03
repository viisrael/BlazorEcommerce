﻿using System.Security.Claims;

namespace BlazorEcommerce.Server.Services.CarServices
{
    public class CartService : ICartService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetUserId()=> int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
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
            cartItems.ForEach(cartItem => cartItem.UserId = GetUserId());

            _context.CartItems.AddRange(cartItems);
            await _context.SaveChangesAsync();

            return await GetCartProductAsync(await _context.CartItems.Where(ci => ci.UserId.Equals(GetUserId())).ToListAsync());
        }

        public async Task<ServiceResponse<int>> GetCartItemsCount()
        {
            var count= (await _context.CartItems.Where(ci => ci.UserId.Equals(GetUserId())).ToListAsync()).Count;

            return new ServiceResponse<int>
            {
                Data = count,
            };
        }
    }
}
