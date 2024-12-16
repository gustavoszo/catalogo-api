using CatalogoApi.Data;
using CatalogoApi.Exceptions;
using CatalogoApi.Models;
using CatalogoApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CatalogoApi.Services
{
    public class CategoryService
    {

        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Category> FindAll(int page)
        {
            IEnumerable<Category> categories = _unitOfWork.CategoryRepository.FindAll();
            return categories;
        }

        public IEnumerable<Category> FindAllWithProducts(int page)
        {
            IEnumerable<Category> categories = _unitOfWork.CategoryRepository.FindAllWithProducts();
            return categories;
        }

        public Category FindById(int id)
        {
            Category? category = _unitOfWork.CategoryRepository.FindById(c => c.CategoryId == id);
            if (category == null) throw new EntityNotFoundException($"Category com id '{id}' não encontrado");

            return category;
        }

        public Category Create(Category category)
        {
            _unitOfWork.CategoryRepository.Create(category);
            _unitOfWork.Commit();

            return category;
        }

        public Category Update(int id, Category category)
        {
            Category? savedCategory = FindById(id);

            if(category.Name != null) savedCategory.Name = category.Name;
            if (category.ImageUrl != null) savedCategory.ImageUrl = category.ImageUrl;

            _unitOfWork.CategoryRepository.Update(savedCategory);
            _unitOfWork.Commit();

            return savedCategory;
        }

        public void Delete(int id)
        {
            Category? category = FindById(id);

            _unitOfWork.CategoryRepository.Delete(category);
            _unitOfWork.Commit();
        }

    }
}
