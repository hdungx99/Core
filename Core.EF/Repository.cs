using Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Core.EF
{
    public abstract class Repository<T> : IRepository<T> where T : BaseEntity, new()
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;
        private readonly IDistributedCache _cache;
        public Repository(DbContext context, IDistributedCache cache)
        {
            _context = context;
            _dbSet = _context.Set<T>();
            _cache = cache;
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
            var result = await _cache.GetStringAsync(id.ToString());
            if (result != null)
            {
                return JsonConvert.DeserializeObject<T>(result);
            }
            var response = await _dbSet.FindAsync(id);
            await _cache.SetStringAsync(id.ToString(), JsonConvert.SerializeObject(response));
            return response;
        }

        public async Task<PagingRes?> Paging(PagingReq req)
        {
            if (req.expression != null)
            {
                var data = await _dbSet.Where(req.expression).ToListAsync();
                var cacheKey = new Guid();
                var result = new PagingRes()
                {
                    data = data.Skip(req.page * req.size).Take(req.size),
                    total = data.Count(),
                    cacheKey = cacheKey
                };
                await _cache.SetStringAsync(cacheKey.ToString(), JsonConvert.SerializeObject(result));
                return result;
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
