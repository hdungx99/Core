namespace Core.EF
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public bool Status { get; set; }
    }
}
