using HealthyMomAndBaby.Entity;
using Microsoft.EntityFrameworkCore;

namespace HealthyMomAndBaby.Service
{
    public interface IOrderService
    {
        Task AddOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int id);
        Task<List<Order>> ShowListOrderAsync();
        Task<Order?> GetDetailOrderByIdAsync(int id);
    }
}
