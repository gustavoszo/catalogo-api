using CatalogoApi.Models;

namespace CatalogoApi.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        public Task<IEnumerable<Product>> GetAllFilteredByPriceAsync(int page, double min, double max);
    }
}
