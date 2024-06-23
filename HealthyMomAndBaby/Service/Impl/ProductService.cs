using HealthyMomAndBaby.Entity;
using HealthyMomAndBaby.InterFaces.Repository;

namespace HealthyMomAndBaby.Service.Impl
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        public ProductService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task AddProductAsync(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            await _productRepository.AddAsync(product);
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
            return await _productRepository.GetAsync(id);
        }

        public async Task<List<Product>> ShowListProductAsync()
        {
            return await _productRepository.GetValuesAsync();
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
            existingProduct.Price = product.Price;
            existingProduct.Quantity = product.Quantity;
            existingProduct.Category = product.Category;
            existingProduct.ProductCategoryId = product.ProductCategoryId;

            _productRepository.Update(existingProduct);  // Update method is synchronous
            await _productRepository.SaveChangesAsync();
        }
    }
}
