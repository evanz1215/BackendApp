namespace Domain.Common
{
    public class EntityBase<TId> : IEntity<TId>
    {
        public TId Id { get; set; }
    }
}