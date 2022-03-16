namespace Domain.Common
{
    public abstract class MyEntity<TId> : IEntity<TId>, ISoftDeletable, IAudited
    {
        public TId Id { get; set; }

        public string CreatedBy { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public string LastModifiedBy { get; private set; }

        public DateTime? LastModifiedAt { get; private set; }

        public string DeletedBy { get; private set; }

        public DateTime? DeletedAt { get; private set; }
    }
}