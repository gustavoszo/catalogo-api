using CatalogoApi.Data;
using CatalogoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogoApi.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext dbContext) : base(dbContext) {}

        public IEnumerable<Category> FindAllWithProducts()
        {
            return DbContext.Categories.Include(c => c.Products).ToList();
        }
    }
}
