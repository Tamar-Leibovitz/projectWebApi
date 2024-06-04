using System.Text.Json;
using Entities;
using Microsoft.Extensions.Logging;
using Repositories;
namespace Services
{
    public class OrderService : IOrderService
    {
        private IOrderRepository _orderRepository;
        private IProductRepository _productRepository;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _logger = logger;
        }
        public async Task<Order> createOrder(Order order)
        {
            int totalPrice = await checkEqualPrice(order);
            order.Sum = totalPrice;
            return await _orderRepository.createOrder(order);
        }

        private async Task<int> checkEqualPrice(Order order)
        {
            int totalPrice = 0;
            foreach (var orderItem in order.OrderItems)
            {
                Product p = await _productRepository.GetProductById(orderItem.ProductId);
                totalPrice += p.Price;
            }

            if (totalPrice != order.Sum)
            {
                _logger.LogError("The user with userID: {0} tryed to change order sum to {1} instead of {2}", order.UserId, order.Sum, totalPrice);
            }
            return totalPrice;
        }





    }
}

    

