using HealthyMomAndBaby.Entity;
using HealthyMomAndBaby.InterFaces.Repository;
using HealthyMomAndBaby.Models.Request;
using Microsoft.EntityFrameworkCore;

namespace HealthyMomAndBaby.Service.Impl
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Account> _accountRepository; 
        public OrderService(IRepository<Order> orderRepository, IRepository<Product> productRepository, IRepository<Account> accountRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _accountRepository = accountRepository;
        }

        public async Task AddOrderAsync(CheckoutRequest order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            List<OrderDetail> orderDetails = new List<OrderDetail>();

            foreach (var cart in order.CartRequests)
            {

                var product = await _productRepository.Get().FirstAsync(x => x.Id == cart.ProductId);

                var orderDetail = new OrderDetail
                {
                    Image = cart.Image,
                    ProductName = cart.ProductName,
                    Quantity = cart.Quantity,
                    SubPrice = cart.TotoalPrice,
                    UnitPrice = cart.Price,
                    Product = product
                };
                orderDetails.Add(orderDetail);
            }
            var account = await _accountRepository.Get().Where(x => x.Id == order.CustomerId).FirstAsync();

            var newOrder = new Order
            {
                FullName = order.FullName,
                OrderDetails = orderDetails,
                PhoneNumber = order.PhoneNumber,
                Total = order.TotalPrice,
                ShipAddress = order.ShippAddres,
                OrderDate = DateTime.Now,
                User = account
            };

            await _orderRepository.AddAsync(newOrder);
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

        public async Task<List<Order>> GetOrdersByUserId(int userId)
        {
            return await _orderRepository.Get().Include(x => x.User).Where(x => x.User.Id == userId).ToListAsync();
        }



        public async Task<List<Order>> ShowListOrderAsync()
        {
            return await _orderRepository.Get().Include(x => x.User).ToListAsync();
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
            existingOrder.OrderDate = order.OrderDate;
            existingOrder.Total = order.Total;

            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();

        }
    }
}
