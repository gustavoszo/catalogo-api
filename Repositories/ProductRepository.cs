using CatalogoApi.Data;
using CatalogoApi.Models;

namespace CatalogoApi.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext dbContext) : base(dbContext) {}

        public IEnumerable<Product> GetAllFilteredByPrice(int page, double min, double max)
        {
            return FindAll(page).Where(p => p.Price >= min && p.Price <= max);
        }
    }
}
