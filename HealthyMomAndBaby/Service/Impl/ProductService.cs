using HealthyMomAndBaby.Entity;
using HealthyMomAndBaby.InterFaces.Repository;
using HealthyMomAndBaby.Models.Request;
using Microsoft.EntityFrameworkCore;

namespace HealthyMomAndBaby.Service.Impl
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductCategory> _productCategoryRepository;
        public ProductService(IRepository<Product> productRepository, IRepository<ProductCategory> productCategoryRepository)
        {
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
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

        public async Task UpdateProductAsync(Product product)
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

            existingProduct.ProductName = product.ProductName;
            existingProduct.Description = product.Description;
            existingProduct.Image = product.Image;
            existingProduct.Price = product.Price;
            existingProduct.Quantity = product.Quantity;

            _productRepository.Update(existingProduct);  // Update method is synchronous
            await _productRepository.SaveChangesAsync();
        }
    }
}
