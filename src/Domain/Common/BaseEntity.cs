namespace Domain.Common
{
    public class BaseEntity<TId> : IEntity<TId>
    {
        public TId Id { get; set; }

    }
}