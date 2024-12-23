using CatalogoApi.Models;

namespace CatalogoApi.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {

        Task<IEnumerable<Category>> FindAllWithProductsAsync(int page);

        Task<IEnumerable<Category>> FindAllFilteredByNameAsync(int page,string name);

    }
}
