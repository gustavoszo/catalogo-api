using System.ComponentModel.DataAnnotations;
using AutoMapper;
using CatalogoApi.Dtos;
using CatalogoApi.Exceptions;
using CatalogoApi.Models;
using CatalogoApi.Repositories;
using Microsoft.AspNetCore.JsonPatch;

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

        public async Task<IEnumerable<Product>> FindAllAsync(int page)
        {
            IEnumerable<Product> products = await _unitOfWork.ProductRepository.FindAllAsync(page);
            return products;
        }


        public async Task<IEnumerable<Product>> FindAllByPriceAsync(int page, double min, double max)
        {
            IEnumerable<Product> products = await _unitOfWork.ProductRepository.GetAllFilteredByPriceAsync(page, min, max);
            return products;
        }

        public async Task<Product> FindByIdAsync(int id)
        {
            Product? product = await _unitOfWork.ProductRepository.FindByIdAsync(p => p.ProductId == id);
            if (product == null) throw new EntityNotFoundException($"Product com id '{id}' não encontrado");

            return product;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            Category category = await _categoryService.FindByIdAsync(product.CategoryId.Value);

            _unitOfWork.ProductRepository.Create(product);
            product.Category = category;

            await _unitOfWork.Commit();

            return product;
        }

        public async Task<Product> UpdateAsync(int id, Product product)
        {
            Product? savedProduct = await FindByIdAsync(id);

            // Atualiza todas as propriedades modificáveis automaticamente
            // _dbContext.Entry(savedProduct).CurrentValues.SetValues(product);
            
            if (product.CategoryId != null) savedProduct.Category = await _categoryService.FindByIdAsync(product.CategoryId.Value);
            if (product.QuantityAvaiable != null) savedProduct.QuantityAvaiable = product.QuantityAvaiable;
            if (product.Price != null) savedProduct.Price = product.Price;
            if (product.Name != null) savedProduct.Name = product.Name;
            if (product.ImageUrl != null) savedProduct.ImageUrl = product.ImageUrl;


            _unitOfWork.ProductRepository.Update(savedProduct);
            await _unitOfWork.Commit();

            return savedProduct;
        }
        public async Task<Product> PartialUpdateAsync(int id, JsonPatchDocument<ProductUpdateDto> patch)
        {
            Product? product = await _unitOfWork.ProductRepository.FindByIdAsync(p => p.ProductId == id);
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
            await _unitOfWork.Commit();

            return product;
        }


        public async Task DeleteAsync(int id)
        {
            Product? product = await FindByIdAsync(id);
            _unitOfWork.ProductRepository.Delete(product);
            await _unitOfWork.Commit();
        }


    }
}
