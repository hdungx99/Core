using Core.Model;
using System.Linq.Expressions;

namespace Core.EF
{
    public interface IRepository<T> where T : BaseEntity, new()
    {
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
        Task CreateListAsync(IEnumerable<T> entities);
        Task UpdateListAsync(IEnumerable<T> entities);
        Task DeleteListAsync(IEnumerable<Guid?> ids);
        Task<T?> GetAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> expression);
        Task<PagingRes?> Paging(PagingReq req);
    }
}
