using HealthyMomAndBaby.Entity;
using HealthyMomAndBaby.InterFaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace HealthyMomAndBaby.Service.Impl
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IRepository<OrderDetail> _orderDetailRepository;
        public OrderDetailService(IRepository<OrderDetail> orderDetailRepository)
        {
            _orderDetailRepository = orderDetailRepository;
        }

        public async Task AddOrderDetailAsync(OrderDetail orderDetail)
        {
            if (orderDetail == null)
            {
                throw new ArgumentNullException(nameof(orderDetail));
            }

            await _orderDetailRepository.AddAsync(orderDetail);
            await _orderDetailRepository.SaveChangesAsync();
        }

        public async Task DeleteOrderDetailAsync(int id)
        {
            var orderDetail = await _orderDetailRepository.GetAsync(id);
            if (orderDetail == null)
            {
                throw new InvalidOperationException($"Order Detail with id {id} not found.");
            }

            _orderDetailRepository.Delete(orderDetail);
            await _orderDetailRepository.SaveChangesAsync();
        }

		public async Task<List<OrderDetail>> GetDetailByOrderId(int orderId)
		{
			return await _orderDetailRepository.Get().Where(x => x.Order.Id == orderId).ToListAsync();
		}

		public async Task<OrderDetail?> GetDetailOrderDetailByIdAsync(int id)
        {
            return await _orderDetailRepository.GetAsync(id);
        }

        public async Task<List<OrderDetail?>> ShowListOrderDetailAsync()
        {
            return await _orderDetailRepository.GetValuesAsync();
        }

        public async Task UpdateOrderDetailAsync(OrderDetail orderDetail)
        {
            if (orderDetail == null)
            {
                throw new ArgumentNullException(nameof(orderDetail));
            }

            var existingOrderDetail = await _orderDetailRepository.GetAsync(orderDetail.Id);
            if (orderDetail == null)
            {
                throw new InvalidOperationException($"Order Detail with id {orderDetail.Id} not found.");
            }
            existingOrderDetail.OrderId = orderDetail.OrderId;
            existingOrderDetail.Quantity = orderDetail.Quantity;
            existingOrderDetail.UnitPrice = orderDetail.UnitPrice;

            _orderDetailRepository.Update(orderDetail);
            await _orderDetailRepository.SaveChangesAsync();
        }
    }
}
