using HealthyMomAndBaby.Entity;
using HealthyMomAndBaby.Models.Request;

namespace HealthyMomAndBaby.Service
{
    public interface IOrderService
    {
        Task AddOrderAsync(CheckoutRequest order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int id);
        Task<List<Order>> ShowListOrderAsync();
        Task<List<Order>> GetOrdersByUserId(int userId);
        Task<Order?> GetDetailOrderByIdAsync(int id);
    }
}
