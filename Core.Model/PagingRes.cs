namespace Core.Model
{
    public class PagingRes
    {
        public IEnumerable<BaseEntity>? data { get; set; }
        public int total { get; set; }
        public int page { get; set; }
        public int size { get; set; }
    }
}
