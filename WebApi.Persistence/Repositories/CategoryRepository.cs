using WebApi.Application.Contracts.Persistence;
using WebApi.Domain;

namespace WebApi.Persistence.Repositories
{

    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private WebApiDbContext DbContext { get; }

        public CategoryRepository(WebApiDbContext dbContext) : base(dbContext)
        {
            DbContext = dbContext;
        }
    }
}