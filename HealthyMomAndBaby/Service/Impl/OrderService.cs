using HealthyMomAndBaby.Common;
using HealthyMomAndBaby.Entity;
using HealthyMomAndBaby.InterFaces.Repository;
using HealthyMomAndBaby.Models.Request;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using System;

namespace HealthyMomAndBaby.Service.Impl
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Account> _accountRepository;
        private readonly IAccountService _accountService;
        private readonly SmtpSettings _smtpsetting;

        public OrderService(IRepository<Order> orderRepository, IRepository<Product> productRepository, IRepository<Account> accountRepository, IOptions<SmtpSettings> smtpsetting, IAccountService accountService)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _accountRepository = accountRepository;
            _smtpsetting = smtpsetting.Value;
            _accountService = accountService;
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
            account.Point = account.Point + (int) Math.Round(order.TotalPrice / 1000);

            await _orderRepository.AddAsync(newOrder);
            await _orderRepository.SaveChangesAsync();

            await SendResetPasswordEmail(account.Id);
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

        public async Task<bool> SendResetPasswordEmail(int id)
        {
            var account = await _accountService.GetDetailProductAsync(id);
            if (account == null)
            {
                return false;
            }
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("HealthyMomAndBaby System", _smtpsetting.Username));
            message.To.Add(new MailboxAddress("", account.Email));
            message.Subject = "Reset Your Password";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $@"
                <p>Dear {account.UserName},</p>
                <p>Your Order has been successfully ordered.</p>
                <p>If you did not make this change, please contact support immediately.</p>
                <p>Thank you,</p>
            ";

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_smtpsetting.SmtpServer, _smtpsetting.Port, _smtpsetting.UseSsl);
                    client.Authenticate(_smtpsetting.Username, _smtpsetting.Password);
                    await client.SendAsync(message);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send email: {ex.Message}");
                    return false;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                }
            }
        }
    }
}
