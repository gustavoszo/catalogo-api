using CatalogoApi.Data;
using CatalogoApi.Exceptions;
using CatalogoApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CatalogoApi.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {

        protected readonly AppDbContext DbContext;

        public Repository(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public IQueryable<T> FindAll(int page)
        {
            return DbContext.Set<T>().Skip(5*page).Take(5).AsNoTracking();
        }
        public T Create(T entity)
        {
            DbContext.Set<T>().Add(entity);

            return entity;
        }

        public T Update(T entity)
        {
            DbContext.Set<T>().Update(entity);

            return entity;
        }

        public T FindById(Expression<Func<T, bool>> predicate)
        {
            return DbContext.Set<T>().AsNoTracking().FirstOrDefault(predicate);
        }

        public void Delete(T entity)
        {
            DbContext.Set<T>().Remove(entity);
        }
    }
}
