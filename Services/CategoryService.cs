using CatalogoApi.Data;
using CatalogoApi.Exceptions;
using CatalogoApi.Models;
using CatalogoApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CatalogoApi.Services
{
    public class CategoryService
    {

        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IEnumerable<Category> FindAll(int page)
        {
            IEnumerable<Category> categories = _categoryRepository.FindAll();
            return categories;
        }

        public IEnumerable<Category> FindAllWithProducts(int page)
        {
            IEnumerable<Category> categories = _categoryRepository.FindAllWithProducts();
            return categories;
        }

        public Category FindById(int id)
        {
            Category? category = _categoryRepository.FindById(c => c.CategoryId == id);
            if (category == null) throw new EntityNotFoundException($"Category com id '{id}' não encontrado");

            return category;
        }

        public Category Create(Category category)
        {
            _categoryRepository.Create(category);

            return category;
        }

        public Category Update(int id, Category category)
        {
            Category? savedCategory = FindById(id);

            if(category.Name != null) savedCategory.Name = category.Name;
            if (category.ImageUrl != null) savedCategory.ImageUrl = category.ImageUrl;

            _categoryRepository.Update(savedCategory);

            return savedCategory;
        }

        public void Delete(int id)
        {
            Category? category = FindById(id);

            _categoryRepository.Delete(category);
        }

    }
}
