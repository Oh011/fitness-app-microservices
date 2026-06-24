namespace AuthenticationService.Application.Abstractions.Persistence
{
    public interface IUnitOfWork : IDisposable
    {


         Task BeginTransactionAsync();

         Task CommitTransactionAsync();

         Task RollbackTransactionAsync();

        public Task<int> Commit(CancellationToken cancellationToken = default);


        public Task ExecuteAsync(Func<Task> action);

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

    }
}
