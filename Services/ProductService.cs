using CatalogoApi.Data;
using CatalogoApi.Exceptions;
using CatalogoApi.Models;
using CatalogoApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CatalogoApi.Services
{
    public class ProductService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly CategoryService _categoryService;

        public ProductService(IUnitOfWork unitOfWork, CategoryService categoryService)
        {
            _unitOfWork = unitOfWork;
            _categoryService = categoryService;
        }

        public IEnumerable<Product> FindAll(int page)
        {
            IEnumerable<Product> products = _unitOfWork.ProductRepository.FindAll();
            return products;
        }

        public Product FindById(int id)
        {
            Product? product = _unitOfWork.ProductRepository.FindById(p => p.ProductId == id);
            if (product == null) throw new EntityNotFoundException($"Product com id '{id}' não encontrado");

            return product;
        }

        public Product Create(Product product)
        {
            product.Category = _categoryService.FindById(product.CategoryId.Value);
            _unitOfWork.ProductRepository.Create(product);
            _unitOfWork.Commit();

            return product;
        }

        public Product Update(int id, Product product)
        {
            Product? savedProduct = FindById(id);

            // Atualiza todas as propriedades modificáveis automaticamente
            // _dbContext.Entry(savedProduct).CurrentValues.SetValues(product);
            
            if (product.CategoryId != null) savedProduct.Category = _categoryService.FindById(product.CategoryId.Value);
            if (product.QuantityAvaiable != null) savedProduct.QuantityAvaiable = product.QuantityAvaiable;
            if (product.Price != null) savedProduct.Price = product.Price;
            if (product.Name != null) savedProduct.Name = product.Name;
            if (product.ImageUrl != null) savedProduct.ImageUrl = product.ImageUrl;


            _unitOfWork.ProductRepository.Update(savedProduct);
            _unitOfWork.Commit();

            return savedProduct;
        }

        public void Delete(int id)
        {
            Product? product = FindById(id);
            _unitOfWork.ProductRepository.Delete(product);
            _unitOfWork.Commit();
        }


    }
}
