namespace BlazorEcommerce.Server.Services.CarServices
{
    public class CartService : ICartService
    {
        private readonly DataContext _context;

        public CartService(DataContext context)
        {
            _context = context;
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
    }
}
