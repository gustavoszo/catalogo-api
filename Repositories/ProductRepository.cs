using CatalogoApi.Data;
using CatalogoApi.Models;

namespace CatalogoApi.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext dbContext) : base(dbContext) {}

    }
}
