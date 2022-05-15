﻿using BlazorEcommerce.Server.Services.CarServices;
using System.Security.Claims;

namespace BlazorEcommerce.Server.Services.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly DataContext _context;
        private readonly ICartService _carteService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderService(DataContext context, ICartService carteService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _carteService = carteService;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        public async Task<ServiceResponse<bool>> PlaceOrder()
        {
            var products = (await _carteService.GetDbCartProducts()).Data;
            decimal totalPrice = 0;

            products.ForEach(product => totalPrice += product.Price * product.Quantity);

            var orderItems = new List<OrderItem>();
            products.ForEach(product => orderItems.Add(new OrderItem { 
                ProductId = product.ProductId, 
                ProductTypeId = product.ProductTypeId,
                Quantity = product.Quantity,
                TotalPrice = product.Price * product.Quantity,
                }  ));

            var order = new Order
            {
                UserId = GetUserId(),
                OrderDate = DateTime.Now,
                TotalPrice = totalPrice,
                OrderItems = orderItems
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };   

        }
    }
}