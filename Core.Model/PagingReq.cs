using System.Linq.Expressions;

namespace Core.Model
{
    public class PagingReq
    {
        public Expression<Func<BaseEntity, bool>> expression { get; set; }
        public int page { get; set; }
        public int size { get; set; }
        public Guid? cacheKey { get; set; }
    }
}
