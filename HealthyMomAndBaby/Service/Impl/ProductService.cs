using HealthyMomAndBaby.Entity;
using HealthyMomAndBaby.InterFaces.Repository;
using HealthyMomAndBaby.Models.Request;
using HealthyMomAndBaby.Common;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MailKit.Net.Smtp;

namespace HealthyMomAndBaby.Service.Impl
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductCategory> _productCategoryRepository;
        private readonly IRepository<Account> _accountRepository;
        private readonly IAccountService _accountService;
        //private readonly SmtpSettings _smtpsetting;

        public ProductService(IRepository<Product> productRepository, IRepository<ProductCategory> productCategoryRepository/*, SmtpSettings smtpSettings*/, IRepository<Account> accountRepository, IAccountService accountService)
        {
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
            //_smtpsetting = smtpSettings;
            _accountRepository = accountRepository;
            _accountService = accountService;
        }
        public async Task AddProductAsync(CreateProduct product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            var category = await _productCategoryRepository.Get().FirstAsync(x => x.Id == product.ProductCategoryId);
            var newProduct = new Product
            {
                Image = product.Image,
                Description = product.Description,
                ProductCategory = category,
                IsDeleted = false,
                Price = double.Parse(product.Price),
                ProductName = product.ProductName,
                Quantity = product.Quantity,
            };
            await _productRepository.AddAsync(newProduct);
            await _productRepository.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetAsync(id);
            if (product == null)
            {
                throw new InvalidOperationException($"Product with id {id} not found.");
            }

            product.IsDeleted = true;
            _productRepository.Update(product);  // Update method is synchronous
            await _productRepository.SaveChangesAsync();
        }

        public async Task<Product?> GetDetailProductAsync(int id)
        {
            return await _productRepository.Get().Include(x => x.ProductCategory).FirstAsync(x => x.Id == id);
        }

        public async Task<List<Product>> GetProductIsAvailable()
        {
            return await _productRepository.Get().Include(x => x.ProductCategory).Where(x => x.IsDeleted == false).ToListAsync();
        }

        public async Task<List<Product>> ShowListProductAsync()
        {
            return await _productRepository.Get().Include(x => x.ProductCategory).ToListAsync();
        }

        public async Task UpdateProductAsync(UpdateProduct product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            var existingProduct = await _productRepository.GetAsync(product.Id);
            if (existingProduct == null)
            {
                throw new InvalidOperationException($"Product with id {product.Id} not found.");
            }
            var category = await _productCategoryRepository.Get().FirstAsync(x => x.Id == product.CategoryId);
            existingProduct.ProductName = product.ProductName;
            existingProduct.Description = product.Description;
            existingProduct.Image = product.Image;
            existingProduct.Price = product.Price;
            existingProduct.IsDeleted = product.IsDeleted;
            existingProduct.Quantity = product.Quantity;
            existingProduct.ProductCategory = category;

            _productRepository.Update(existingProduct);  // Update method is synchronous
            await _productRepository.SaveChangesAsync();
        }




        //private async Task SendAddProductEmail(int Id, string productNamme)
        //{
        //    var member = await _accountService.GetDetailProductAsync(Id);
        //    if (member == null || string.IsNullOrEmpty(member.Email))
        //    {
        //        throw new Exception($"Member with FeId {Id} not found or has no email specified.");
        //    }

        //    var message = new MimeMessage();
        //    message.From.Add(new MailboxAddress("ExchangeGood System", _smtpsetting.Username));
        //    message.To.Add(new MailboxAddress(member.UserName, member.Email));
        //    message.Subject = "New Report sended";

        //    var bodyBuilder = new BodyBuilder();
        //    bodyBuilder.HtmlBody = $@"
        //    <p>Dear {member.UserName},</p>
        //    <p>We are pleased to inform you that your report to Product: <strong>{productNamme}</strong> has been successfully approved to ExchangeGood.</p>
        //    <p>If you have any feedbacks, please respond to this mail!</p>
        //    <p>Thank you for using our platform!</p>
        //    <p>Best regards,</p>
        //    <p>The ExchangeGood Team</p>
        //";

        //    message.Body = bodyBuilder.ToMessageBody();

        //    using (var client = new SmtpClient())
        //    {
        //        try
        //        {
        //            client.Connect(_smtpsetting.SmtpServer, _smtpsetting.Port, _smtpsetting.UseSsl);
        //            client.Authenticate(_smtpsetting.Username, _smtpsetting.Password);
        //            await client.SendAsync(message);
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"Failed to send email: {ex.Message}");
        //            throw;
        //        }
        //        finally
        //        {
        //            await client.DisconnectAsync(true);
        //        }
        //    }
        //}
    }
}
