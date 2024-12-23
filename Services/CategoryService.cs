using CatalogoApi.Exceptions;
using CatalogoApi.Models;
using CatalogoApi.Repositories;

namespace CatalogoApi.Services
{
    public class CategoryService
    {

        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Category>> FindAllAsync(int page)
        {
            IEnumerable<Category> categories = await _unitOfWork.CategoryRepository.FindAllAsync(page);
            return categories;
        }

        public async Task<IEnumerable<Category>> FindAllWithProductsAsync(int page)
        {
            IEnumerable<Category> categories = await _unitOfWork.CategoryRepository.FindAllWithProductsAsync(page);
            return categories;
        }

        public async Task<IEnumerable<Category>> FindAllFilteredByNameAsync(int page, string name)
        {
            IEnumerable<Category> categories = await _unitOfWork.CategoryRepository.FindAllFilteredByNameAsync(page, name);
            return categories;
        }

        public async Task<Category> FindByIdAsync(int id)
        {
            Category? category = await _unitOfWork.CategoryRepository.FindByIdAsync(c => c.CategoryId == id);
            if (category == null) throw new EntityNotFoundException($"Category com id '{id}' não encontrado");

            return category;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            _unitOfWork.CategoryRepository.Create(category);
            await _unitOfWork.Commit();

            return category;
        }

        public async Task<Category> UpdateAsync(int id, Category category)
        {
            Category? savedCategory = await FindByIdAsync(id);

            if(category.Name != null) savedCategory.Name = category.Name;
            if (category.ImageUrl != null) savedCategory.ImageUrl = category.ImageUrl;

            _unitOfWork.CategoryRepository.Update(savedCategory);
            await _unitOfWork.Commit();

            return savedCategory;
        }

        public async Task Delete(int id)
        {
            Category? category = await FindByIdAsync(id);

            _unitOfWork.CategoryRepository.Delete(category);
            await _unitOfWork.Commit();
        }

    }
}
