using CatalogoApi.Data;
using CatalogoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogoApi.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext dbContext) : base(dbContext) {}

        public async Task<IEnumerable<Product>> GetAllFilteredByPriceAsync(int page, double min, double max)
        {
            return await DbContext.Set<Product>()
                          .Where(p => p.Price >= min && p.Price <= max)
                          .Skip(5 * page)
                          .Take(5)
                          .AsNoTracking()
                          .ToListAsync();
        }
    }
}
