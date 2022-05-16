using BlazorEcommerce.Server.Services.CarServices;
using Stripe;
using Stripe.Checkout;

namespace BlazorEcommerce.Server.Services.PaymentServices
{
    public class PaymentService : IPaymentService
    {
        private readonly ICartService _cartService;
        private readonly IAuthService _authService;
        private readonly IOrderService _orderService;

        const string secret = "whsec_e2be9eb2e7ba5680ae438e0142d0a744bc254db258a3ed695fe1d69b7bf41812";

        public PaymentService(ICartService cartService, IAuthService authService, IOrderService orderService)
        {
            StripeConfiguration.ApiKey = "sk_test_51KzqIcLdirp6wkS0orC57cOw6sXWsGJARjfTTIojYLXsM6U5Iw92yT0w67XwF27iCSWrZ6BlprjUEUYPmEE8Dldj00j1RsN7TN";

            _cartService = cartService;
            _authService = authService;
            _orderService = orderService;
        }


        public async Task<Session> CreateCheckoutSession()
        {
            var products = (await _cartService.GetDbCartProducts()).Data;
            var lineItems = new List<SessionLineItemOptions>();

            products.ForEach(product => lineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmountDecimal = product.Price *100,
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = product.Title,
                        Images = new List<string> { product.ImageUrl}
                    }
                },
                Quantity = product.Quantity,
            }));

            var options = new SessionCreateOptions
            {
                CustomerEmail = _authService.GetUserEmail(),
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = "https://localhost:7050/order-success",
                CancelUrl = "https://localhost:7050/cart"
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return session;
        }

        public async Task<ServiceResponse<bool>> FulfillOrder(HttpRequest request)
        {
            var json = await new StreamReader(request.Body).ReadToEndAsync();

            try{ 
                var stripeEvent = EventUtility.ConstructEvent(json, request.Headers["Stripe-Signature"], secret);

                if(stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Session;
                    var user = await _authService.GetUserByEmail(session.CustomerDetails.Email);

                    await _orderService.PlaceOrder(user.Id);
                }

                return new ServiceResponse<bool> { Data = true };
            } catch(StripeException e)
            {
                return new ServiceResponse<bool> { Data = false, Success = false, Message = e.Message };
            }
        }
    }
}
