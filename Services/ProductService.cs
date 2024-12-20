using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;
using AutoMapper;
using CatalogoApi.Data;
using CatalogoApi.Dtos;
using CatalogoApi.Exceptions;
using CatalogoApi.Models;
using CatalogoApi.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogoApi.Services
{
    public class ProductService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly CategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;
        
        public ProductService(IUnitOfWork unitOfWork, CategoryService categoryService, IMapper mapper, ILogger<ProductService> logger)
        {
            _unitOfWork = unitOfWork;
            _categoryService = categoryService;
            _logger = logger;
            _mapper = mapper;
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
            Category category = _categoryService.FindById(product.CategoryId.Value);
            _logger.LogInformation(category.ToString());
            _unitOfWork.ProductRepository.Create(product);
            product.Category = category;
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
        public Product PartialUpdate(int id, JsonPatchDocument<ProductUpdateDto> patch)
        {
            Product? product = _unitOfWork.ProductRepository.FindById(p => p.ProductId == id);
            if (product == null) throw new EntityNotFoundException($"Product com id '{id}' não encontrado");
            
            try
            {
                var productUpdateDtoMapped = _mapper.Map<ProductUpdateDto>(product);
                patch.ApplyTo(productUpdateDtoMapped);
                _mapper.Map(productUpdateDtoMapped, product);
            } 
            catch (Exception ex)
            {
                throw new ValidationException(ex.Message);
            }

            _unitOfWork.ProductRepository.Update(product);
            _unitOfWork.Commit();

            return product;
        }


        public void Delete(int id)
        {
            Product? product = FindById(id);
            _unitOfWork.ProductRepository.Delete(product);
            _unitOfWork.Commit();
        }


    }
}
