using HealthyMomAndBaby.InterFaces.Repository;
using HealthyMomAndBaby.Repositories;
using HealthyMomAndBaby.Service;
using HealthyMomAndBaby.Service.Impl;
using Microsoft.Identity.Client;

namespace HealthyMomAndBaby.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static void Register(this IServiceCollection services)
		{
			services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
			services.AddScoped<IAccountService, AccountServiceImpl>();
            services.AddScoped<IVoucherService, VoucherService>();
            services.AddScoped<IProductCategoryService, ProductCategoryService>();
			services.AddScoped<IOrderDetailService, OrderDetailService>();
            services.AddSession();

        }
	}
}
