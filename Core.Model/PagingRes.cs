namespace Core.Model
{
    public class PagingRes
    {
        public IEnumerable<BaseEntity>? data { get; set; }
        public int total { get; set; }
        public Guid cacheKey { get; set; }
    }
}
