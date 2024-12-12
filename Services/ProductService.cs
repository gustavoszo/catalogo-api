using CatalogoApi.Data;
using CatalogoApi.Exceptions;
using CatalogoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogoApi.Services
{
    public class ProductService
    {

        private AppDbContext _dbContext;

        public ProductService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Product> FindAll(int page)
        {
            IEnumerable<Product> products = _dbContext.Products.Skip(5 * page).Take(5).AsNoTracking();
            return products;
        }

        public Product FindById(int id)
        {
            Product? product = _dbContext.Products.AsNoTracking().FirstOrDefault(p => p.ProductId == id);
            if (product == null) throw new EntityNotFoundException($"Product com id '{id}' não encontrado");

            return product;
        }

        public Product Create(Product product)
        {
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();

            return product;
        }

        public Product Update(int id, Product product)
        {
            Product? savedProduct = _dbContext.Categories.AsNoTracking().FirstOrDefault(c => c.ProductId == id);
            if (savedProduct == null) throw new EntityNotFoundException($"Product com id '{id}' não encontrado");

            if (product.Name != null) savedProduct.Name = product.Name;
            if (product.ImageUrl != null) savedProduct.ImageUrl = product.ImageUrl;

            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();

            return product;
        }

        public void Delete(int id)
        {
            Product? product = _dbContext.Products.AsNoTracking().FirstOrDefault(p => p.ProductId == id);
            if (product == null) throw new EntityNotFoundException($"Product com id '{id}' não encontrado");

            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();
        }


    }
}
