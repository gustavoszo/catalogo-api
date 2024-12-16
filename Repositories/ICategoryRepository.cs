using CatalogoApi.Models;

namespace CatalogoApi.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {

        IEnumerable<Category> FindAllWithProducts();

    }
}
