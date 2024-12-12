using CatalogoApi.Data;
using CatalogoApi.Exceptions;
using CatalogoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogoApi.Services
{
    public class CategoryService
    {

        private AppDbContext _dbContext;

        public CategoryService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Category> FindAll(int page)
        {
            IEnumerable<Category> categories = _dbContext.Categories.Skip(5 * page).Take(5).AsNoTracking();
            return categories;
        }

        public Category FindById(int id)
        {
            Category? category = _dbContext.Categories.AsNoTracking().FirstOrDefault(c => c.CategoryId == id);
            if (category == null) throw new EntityNotFoundException($"Category com id '{id}' não encontrado");

            return category;
        }

        public Category Create(Category category)
        {
            _dbContext.Categories.Add(category);
            _dbContext.SaveChanges();

            return category;
        }

        public Category Update(int id, Category category)
        {
            Category? savedCategory = _dbContext.Categories.AsNoTracking().FirstOrDefault(c => c.CategoryId == id);
            if (savedCategory == null) throw new EntityNotFoundException($"Category com id '{id}' não encontrado");

            if(category.Name != null) savedCategory.Name = category.Name;
            if (category.ImageUrl != null) savedCategory.ImageUrl = category.ImageUrl;

            _dbContext.Categories.Add(category);
            _dbContext.SaveChanges();

            return category;
        }

        public void Delete(int id)
        {
            Category? category = _dbContext.Categories.AsNoTracking().FirstOrDefault(c => c.CategoryId == id);
            if (category == null) throw new EntityNotFoundException($"Category com id '{id}' não encontrado");

            _dbContext.Categories.Remove(category);
            _dbContext.SaveChanges();
        }

    }
}
