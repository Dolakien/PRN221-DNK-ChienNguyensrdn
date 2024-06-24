using HealthyMomAndBaby.Entity;
using HealthyMomAndBaby.InterFaces.Repository;

namespace HealthyMomAndBaby.Service.Impl
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> _orderRepository;
        public OrderService(IRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task AddOrderAsync(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _orderRepository.GetAsync(id);
            if (order == null)
            {
                throw new InvalidOperationException($"Order with id {id} not found.");
            }

            _orderRepository.Delete(order);
            await _orderRepository.SaveChangesAsync();
        }

        public async Task<Order?> GetDetailOrderByIdAsync(int id)
        {
            return await _orderRepository.GetAsync(id);

        }

        public async Task<List<Order>> ShowListOrderAsync()
        {
            return await _orderRepository.GetValuesAsync();
        }

        public async Task UpdateOrderAsync(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            var existingOrder = await _orderRepository.GetAsync(order.Id);
            if (order == null)
            {
                throw new InvalidOperationException($"Order with id {order.Id} not found.");
            }
            existingOrder.UserId = order.UserId;
            existingOrder.OrderDate = order.OrderDate;
            existingOrder.Total = order.Total;
            existingOrder.Status = order.Status;

            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();

        }
    }
}
