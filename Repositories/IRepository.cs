using System.Linq.Expressions;

namespace CatalogoApi.Repositories
{
    public interface IRepository<T>
    {

        Task<IEnumerable<T>> FindAllAsync(int page);
        T Create(T entity);
        T Update(T entity);
        Task<T> FindByIdAsync(Expression<Func<T, bool>> predicate);
        void Delete(T entity);

    }
}
