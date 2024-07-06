using HealthyMomAndBaby.Entity;
using HealthyMomAndBaby.Models.Request;

namespace HealthyMomAndBaby.Service
{
    public interface IProductService
    {
        Task AddProductAsync(CreateProduct product);
        Task UpdateProductAsync(UpdateProduct product);
        Task DeleteProductAsync(int id);
        Task<List<Product>> ShowListProductAsync();
        Task<Product?> GetDetailProductAsync(int id);
        Task<List<Product>> GetProductIsAvailable();
    }
}
