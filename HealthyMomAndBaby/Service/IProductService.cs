using HealthyMomAndBaby.Entity;

namespace HealthyMomAndBaby.Service
{
    public interface IProductService
    {
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
        Task<List<Product>> ShowListProductAsync();
        Task<Product?> GetDetailProductAsync(int id);

    }
}
