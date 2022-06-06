using WebApi.Domain;

namespace WebApi.Application.Contracts.Persistence
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Product> GetProductWithDetails(int id);
        Task<List<Product>> GetProductsWithDetails();
    }
}