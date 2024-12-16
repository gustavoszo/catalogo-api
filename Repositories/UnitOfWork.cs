using CatalogoApi.Data;
using Microsoft.EntityFrameworkCore;

namespace CatalogoApi.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private ICategoryRepository? _categoryRepository;
        private IProductRepository? _productRepository;

        private readonly AppDbContext _dbContext;

        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IProductRepository ProductRepository
        {
            get
            {
                return _productRepository = _productRepository ?? new ProductRepository(_dbContext);
            }
        }

        public ICategoryRepository CategoryRepository
        {
            get
            {
                return _categoryRepository = _categoryRepository ?? new CategoryRepository(_dbContext);
            }
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
