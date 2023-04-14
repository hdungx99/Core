using Core.Model;
using Microsoft.EntityFrameworkCore;

namespace Core.EF
{
    public abstract class Repository<T> : IRepository<T> where T : BaseEntity, new()
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;
        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task CreateListAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteListAsync(IEnumerable<Guid?> ids)
        {
            var entities = _dbSet.Where(x => ids.Contains(x.Id));
            if (entities != null)
            {
                _dbSet.RemoveRange(entities);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<T>> Find(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            return await _dbSet.Where(expression).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<PagingRes?> Paging(PagingReq req)
        {
            if (req.expression != null)
            {
                var data = await _dbSet.Where(req.expression).ToListAsync();
                return new PagingRes()
                {
                    data = data,
                    total = data.Count()
                };
            }
            return null;
        }

        public async Task UpdateAsync(T entity)
        {
            await Task.Run(() => _dbSet.Update(entity));
            await _context.SaveChangesAsync();
        }

        public async Task UpdateListAsync(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
            await _context.SaveChangesAsync();
        }
    }
}
