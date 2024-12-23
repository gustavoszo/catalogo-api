using CatalogoApi.Models;

namespace CatalogoApi.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        public IEnumerable<Product> GetAllFilteredByPrice(int page, double min, double max);
    }
}
