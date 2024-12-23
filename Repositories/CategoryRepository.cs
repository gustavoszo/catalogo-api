using CatalogoApi.Data;
using CatalogoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogoApi.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext dbContext) : base(dbContext) {}

        public async Task<IEnumerable<Category>> FindAllFilteredByNameAsync(int page, string name)
        {
            return await DbContext.Set<Category>()
                          .Where(c => c.Name.ToLower().Contains(name.ToLower()))
                          .Skip(5 * page)
                          .Take(5)
                          .AsNoTracking()
                          .ToListAsync();
        }

        public async Task<IEnumerable<Category>> FindAllWithProductsAsync(int page)
        {
            return await DbContext.Categories.Include(c => c.Products).Skip((page - 1) * 5).ToListAsync();
        }
    }
}
