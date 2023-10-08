using System.Linq.Expressions;

namespace FFS.Application.Infrastructure.Interfaces
{
    public interface IRepository<T, K> where T : class
    {
        Task<T> FindById(K id, Expression<Func<T, object>>[] includes);

        Task<T> FindSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

        IQueryable<T> FindAll(params Expression<Func<T, object>>[] includes);

        IQueryable<T> FindAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

        Task<List<T>> GetList(params Expression<Func<T, object>>[] includes);

        Task<List<T>> GetList(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

        Task Add(T entity);

        Task<T> CreateAndGetEntity(T entity);

        Task AddMultiple(List<T> entities);

        Task Update(T entity, params string[] propertiesToExclude);

        Task Update(T entity);

        Task Remove(T entity);

        Task Remove(K id);

        Task RemoveMultiple(List<T> entities);
    }
}
