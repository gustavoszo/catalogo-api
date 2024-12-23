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

        public async Task<IEnumerable<T>> FindAllAsync(int page)
        {
            return await DbContext.Set<T>().Skip(5*page).Take(5).AsNoTracking().ToListAsync();
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

        public async Task<T> FindByIdAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public void Delete(T entity)
        {
            DbContext.Set<T>().Remove(entity);
        }
    }
}
