using HealthyMomAndBaby.Entity;

namespace HealthyMomAndBaby.Service
{
    public interface IOrderDetailService
    {
        Task AddOrderDetailAsync(OrderDetail orderDetail);
        Task UpdateOrderDetailAsync(OrderDetail orderDetail);
        Task DeleteOrderDetailAsync(int id);
        Task<List<OrderDetail?>> ShowListOrderDetailAsync();
        Task<OrderDetail?> GetDetailOrderDetailByIdAsync(int id);
    }
}
