using System.Linq.Expressions;

namespace CatalogoApi.Repositories
{
    public interface IRepository<T>
    {

        IQueryable<T> FindAll();
        T Create(T entity);
        T Update(T entity);
        T FindById(Expression<Func<T, bool>> predicate);
        void Delete(T entity);

    }
}
