using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Application.Contracts.Persistence;
using WebApi.Persistence.Repositories;

namespace WebApi.Persistence
{

    public static class PersistenceServicesRegistration
    {
        public static IServiceCollection ConfigurePersistenceServices(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddDbContext<WebApiDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("WebApiDatabase")));

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            return services;
        }
    }
}