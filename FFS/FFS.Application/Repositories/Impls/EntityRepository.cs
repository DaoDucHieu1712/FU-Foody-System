using FFS.Application.Data;
using FFS.Application.Entities.Common;
using FFS.Application.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FFS.Application.Repositories.Impls
{
    public class EntityRepository <T, K> : IRepository<T, K> where T :BaseEntity<K>
    {
        protected readonly ApplicationDbContext _context;

        public EntityRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(T entity)
        {
            try
            {
                await _context.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<T>> GetList(params Expression<Func<T, object>>[] includeProperties)
        {
            try
            {
                IQueryable<T> items = _context.Set<T>();
                if (includeProperties != null)
                {
                    foreach (var includeProperty in includeProperties)
                    {
                        items = items.Include(includeProperty);
                    }
                }
                return await items.ToListAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task<List<T>> GetList(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {

            try
            {
                IQueryable<T> items = _context.Set<T>();
                if (includeProperties != null)
                {
                    foreach (var includeProperty in includeProperties)
                    {
                        items = items.Include(includeProperty);
                    }
                }
                return await items.Where(predicate).ToListAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task<T> FindById(K id, params Expression<Func<T, object>>[] includeProperties)
        {
            try
            {
                IQueryable<T> items = _context.Set<T>();
                if (includeProperties != null)
                {
                    foreach (var includeProperty in includeProperties)
                    {
                        items = items.Include(includeProperty);
                    }
                }
                return await items.FirstOrDefaultAsync(x => x.Id.Equals(id));
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<T> FindSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            try
            {
                IQueryable<T> items = _context.Set<T>();
                if (includeProperties != null)
                {
                    foreach (var includeProperty in includeProperties)
                    {
                        items = items.Include(includeProperty);
                    }
                }
                return await items.SingleOrDefaultAsync(predicate);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task Remove(T entity)
        {
            try
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task Remove(K id)
        {
            try
            {
                var entity = await FindById(id);
                await Remove(entity);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task RemoveMultiple(List<T> entities)
        {
            try
            {
                _context.Set<T>().RemoveRange(entities);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task Update(T entity, params string[] propertiesToExclude)
        {
            try
            {
                _context.Set<T>().Attach(entity);
                var entry = _context.Entry(entity);
                entry.State = EntityState.Modified;
                foreach (var property in propertiesToExclude)
                {
                    entry.Property(property).IsModified = false;
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task Update(T entity)
        {
            try
            {
                _context.Set<T>().Update(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public IQueryable<T> FindAll(params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> items = _context.Set<T>();
                if (includes != null)
                {
                    foreach (var includeProperty in includes)
                    {
                        items = items.Include(includeProperty);
                    }
                }
                return items;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public IQueryable<T> FindAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> items = _context.Set<T>();
                if (includes != null)
                {
                    foreach (var includeProperty in includes)
                    {
                        items = items.Include(includeProperty);
                    }
                }
                return items.Where(predicate);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task AddMultiple(List<T> entities)
        {
            try
            {
                await _context.Set<T>().AddRangeAsync(entities);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<T> CreateAndGetEntity(T entity)
        {
            try
            {
                IQueryable<T> items = _context.Set<T>();
                await _context.AddAsync(entity);
                await _context.SaveChangesAsync();
                return await items.FirstOrDefaultAsync(x => x.Id.Equals(entity.Id));
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
