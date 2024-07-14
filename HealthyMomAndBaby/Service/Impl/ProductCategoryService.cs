using HealthyMomAndBaby.Entity;
using HealthyMomAndBaby.InterFaces.Repository;
using HealthyMomAndBaby.Models.Request;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

namespace HealthyMomAndBaby.Service.Impl
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IRepository<ProductCategory> _projectCategoryRepository;
        public ProductCategoryService(IRepository<ProductCategory> repository) {
            _projectCategoryRepository = repository;
        }
        public async Task Add(CreateCategory productCategory)
        {
            var category = new ProductCategory
            {
                ProductCategoryName = productCategory.CategoryName,
                IsAvailable = true
            };
           await _projectCategoryRepository.AddAsync(category);
           await _projectCategoryRepository.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var productCategory = await _projectCategoryRepository.GetAsync(id);
            if (productCategory == null)
            {
                throw new InvalidOperationException($"Category with id {id} not found.");
            }

            productCategory.IsAvailable = false;
            _projectCategoryRepository.Update(productCategory);
            await _projectCategoryRepository.SaveChangesAsync();
        }

        public async Task<List<ProductCategory>> GetProductCategories()
        {
           return await _projectCategoryRepository.Get().ToListAsync();
        }

        public async Task Update(UpdateCategory updateCategory)
        {
            var productCategory = await _projectCategoryRepository.GetAsync(updateCategory.Id);
            if (productCategory == null)
            {
                throw new InvalidOperationException($"Category with id {updateCategory.Id} not found.");
            }

            productCategory.ProductCategoryName = updateCategory.Name;
            productCategory.IsAvailable = updateCategory.IsAvailable;

            _projectCategoryRepository.Update(productCategory);
            await _projectCategoryRepository.SaveChangesAsync();
        }
    }
}
