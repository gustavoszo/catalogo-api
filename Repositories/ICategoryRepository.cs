using CatalogoApi.Models;

namespace CatalogoApi.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {

        IEnumerable<Category> FindAllWithProducts(int page);

        IEnumerable<Category> FindAllFilteredByName(int page,string name);

    }
}
