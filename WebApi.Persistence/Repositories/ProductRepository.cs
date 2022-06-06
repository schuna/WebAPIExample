using Microsoft.EntityFrameworkCore;
using WebApi.Application.Contracts.Persistence;
using WebApi.Domain;

namespace WebApi.Persistence.Repositories
{

    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly WebApiDbContext _dbContext;

        public ProductRepository(WebApiDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Product> GetProductWithDetails(int id)
        {
            var product = await _dbContext.Products
                .Include(q => q.Category)
                .FirstOrDefaultAsync(q => q.Id == id);
            return product!;
        }

        public async Task<List<Product>> GetProductsWithDetails()
        {
            var products = await _dbContext.Products
                .Include(q => q.Category)
                .ToListAsync();
            return products;
        }
    }
}