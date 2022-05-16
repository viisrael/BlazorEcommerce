﻿namespace BlazorEcommerce.Server.Services.OrderServices
{
    public interface IOrderService
    {
        Task<ServiceResponse<bool>> PlaceOrder(int userId);
        Task<ServiceResponse<List<OrderOverviewResponse>>> GetOrders();
        Task<ServiceResponse<OrderDetailsResponse>> GetOrdersDetails(int orderId);
    }
}
