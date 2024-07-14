using HealthyMomAndBaby.Entity;
using HealthyMomAndBaby.Models.Request;

namespace HealthyMomAndBaby.Service
{
    public interface IProductCategoryService
    {
        Task<List<ProductCategory>> GetProductCategories();
        Task Add(CreateCategory productCategory);
        Task Update(UpdateCategory productCategory);
        Task Delete(int id);

    }
}
