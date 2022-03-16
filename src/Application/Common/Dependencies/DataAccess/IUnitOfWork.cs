namespace Application.Common.Dependencies.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        bool HasActiveTransaction { get; }

        Task BeginTransactionAsync();

        Task CommitTransactionAsync();

        Task RollbackTransactionAsync();

        Task<int> SaveChangesAsync();
    }
}