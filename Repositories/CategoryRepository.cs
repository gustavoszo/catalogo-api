using CatalogoApi.Data;
using CatalogoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogoApi.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext dbContext) : base(dbContext) {}

        public IEnumerable<Category> FindAllFilteredByName(int page, string name)
        {
            return FindAll(page).Where(c => c.Name.ToLower().Contains(name.ToLower()));
        }

        public IEnumerable<Category> FindAllWithProducts(int page)
        {
            return DbContext.Categories.Include(c => c.Products).Skip((page - 1) * 5).ToList();
        }
    }
}
