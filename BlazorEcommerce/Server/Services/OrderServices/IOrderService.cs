namespace BlazorEcommerce.Server.Services.OrderServices
{
    public interface IOrderService
    {
        Task<ServiceResponse<bool>> PlaceOrder();
    }
}
