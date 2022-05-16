using Stripe.Checkout;

namespace BlazorEcommerce.Server.Services.PaymentServices
{
    public interface IPaymentService
    {
        Task<Session> CreateCheckoutSession();
        Task<ServiceResponse<bool>> FulfillOrder(HttpRequest httpRequest);
    }
}