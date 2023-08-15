using DataAccess.Data;
using DataAccess.Models;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        // By using ReadOnly ApplicationDbContext, you can have access to only
        // querying capabilities of DbContext. UnitOfWork actually writes
        // (commits) to the PHYSICAL tables (not internal object).
        private readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

        }

        public void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            _dbContext.SaveChanges();
        }

        public void Delete(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
            _dbContext.SaveChanges();
        }

        public virtual T Get(Expression<Func<T, bool>> predicate, bool trackChanges = false, string includes = null)
        {
            if (includes == null)   // we are not joining any objects
            {
                if (!trackChanges) // is set to false, we do not want EF tracking changes
                {
                    return _dbContext.Set<T>()
                      .Where(predicate)
                      .AsNoTracking()
                      .FirstOrDefault();
                }

                else // EF defaults of tracking changes are assumed
                {
                    return _dbContext.Set<T>()
                      .Where(predicate)
                      .FirstOrDefault();
                }
            }

            else // If other objects to include (join)
                 // includes = "Comma,Separated,Objects,Without,Spaces"
            {
                IQueryable<T> queryable = _dbContext.Set<T>();

                foreach (var includeProperty in includes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    queryable = queryable.Include(includeProperty);
                }

                if (!trackChanges) // is set to false, we're not tracking changes
                {
                    return queryable
                      .Where(predicate)
                      .AsNoTracking()
                      .FirstOrDefault();
                }

                else // EF is tracking changes
                {
                    return queryable
                      .Where(predicate)
                      .FirstOrDefault();
                }
            }
        }

        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> predicate, bool trackChanges = false, string includes = null)
        {
            if (includes == null)   // we are not joining any objects
            {
                if (!trackChanges) // is set to false, we're not tracking changes
                {
                    return await _dbContext.Set<T>()
                      .Where(predicate)
                      .AsNoTracking()
                      .FirstOrDefaultAsync();
                }

                else // EF is tracking changes
                {
                    return await _dbContext.Set<T>()
                      .Where(predicate)
                      .FirstOrDefaultAsync();
                }
            }

            else // If other objects to include (join)
                 // includes = "Address, Finances,Dependents"
            {
                IQueryable<T> queryable = _dbContext.Set<T>();

                foreach (var includeProperty in includes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    queryable = queryable.Include(includeProperty);
                }

                if (!trackChanges) // is set to false, EF is not tracking changes
                {
                    return await queryable
                      .Where(predicate)
                      .AsNoTracking()
                      .FirstOrDefaultAsync();
                }

                else // EF is tracking changes
                {
                    return await queryable
                      .Where(predicate)
                      .FirstOrDefaultAsync();
                }
            }
        }

        // The virtual keyword is used to modify a method, property, indexer, or
        // and allows for it to be overridden in a derived class.
        public virtual T GetById(int? id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null, Expression<Func<T, int>> orderBy = null, string includes = null)
        {
            IQueryable<T> queryable = _dbContext.Set<T>();
            if (predicate != null && includes == null)
            {
                return _dbContext.Set<T>()
                    .Where(predicate)
                    .AsEnumerable();
            }
            // has optional includes
            else if (includes != null)
            {
                foreach (var includeProperty in includes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    queryable = queryable.Include(includeProperty);
                }
            }

            if (predicate == null)
            {
                if (orderBy == null)
                {
                    return queryable.AsEnumerable();
                }
                else
                {
                    return await queryable.OrderBy(orderBy).ToListAsync();
                }
            }
            else
            {
                if (orderBy == null)
                {

                    if (orderBy == null)
                    {

                        return queryable.Where(predicate).ToList();

                    }
                    else
                    {
                        return queryable.Where(predicate).OrderBy(orderBy).ToList();
                    }

                }
                else
                {
                    return await queryable.Where(predicate).OrderBy(orderBy).ToListAsync();
                }
            }
        }

        public void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate = null, Expression<Func<T, int>> orderBy = null, string includes = null)
        {
            IQueryable<T> queryable = _dbContext.Set<T>();
            if (predicate != null && includes == null) // Where clause but no join
            {
                return _dbContext.Set<T>()
                    .Where(predicate)
                    .AsEnumerable();
            }
            // has optional includes
            else if (includes != null)
            {
                foreach (var includeProperty in includes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    queryable = queryable.Include(includeProperty);
                }
            }

            if (predicate == null)
            {
                if (orderBy == null)
                {
                    return queryable.AsEnumerable();
                }
                else
                {
                    return queryable.OrderBy(orderBy).ToList();
                }
            }
            else
            {
                if (orderBy == null)
                {

                    return queryable.Where(predicate).ToList();

                }
                else
                {
                    return queryable.Where(predicate).OrderBy(orderBy).ToList();
                }
            }

        }
        public int DecrementCount(ShoppingCart shoppingCart, int count)
        {
            shoppingCart.Count -= count;
            return shoppingCart.Count;
        }

        public int IncrementCount(ShoppingCart shoppingCart, int count)
        {
            shoppingCart.Count += count;
            return shoppingCart.Count;
        }

    }
}
