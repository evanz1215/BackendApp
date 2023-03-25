namespace Domain.Common
{
    public class BaseEntity<TId> : IEntity<TId>
    {
        public TId Id { get; set; }
        public int MyProperty { get; set; }
    }
}