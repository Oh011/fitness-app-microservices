
using AuthenticationService.Application.Abstractions.Persistence;
using AuthenticationService.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Concurrent;

namespace AuthenticationService.Infrastructure.Persistence.Repositories
{
    internal class UnitOfWork : IUnitOfWork 
    {


        private readonly ConcurrentDictionary<string, object> _repositories;
        private readonly ApplicationDbContext _context;
        private int _depth = 0;
        private IDbContextTransaction? _transaction;




        public UnitOfWork(ApplicationDbContext context )
        {
            _context = context;
            _repositories = new ConcurrentDictionary<string, object>();

        }

        public async Task<int> Commit(CancellationToken cancellationToken = default)
                => await _context.SaveChangesAsync();



        public async Task ExecuteAsync(Func<Task> action)
        {
            if (_depth == 0 && _transaction == null)
            {
                _transaction = await _context.Database.BeginTransactionAsync();
            }
            _depth++;
            try
            {
                await action();
                _depth--;
                if (_depth == 0)
                {
                   
                    await _transaction!.CommitAsync();
                    _transaction.Dispose();
                    _transaction = null; // ← reset
                }
            }
            catch
            {
                _depth--;
                if (_depth == 0)
                {
                    await _transaction!.RollbackAsync();
                    _transaction.Dispose();
                    _transaction = null; // ← reset
                }
                throw;
            }
        }



        public async Task BeginTransactionAsync()
        {
            if (_transaction != null) return; // Prevent nested transaction crashes
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction == null) throw new InvalidOperationException("No active transaction to commit.");

            try
            {

                await _transaction.CommitAsync();
            }
            finally
            {
                await DisposeTransactionAsync();
            }
        }

        public async Task RollbackTransactionAsync()
        {
            // Safe check: If the underlying connection dropped, EF Core may have already disposed it
            if (_transaction == null) return;

            try
            {
                await _transaction.RollbackAsync();
            }
            catch
            {
                // Suppress or log rollback failures if the DB connection is completely dead
            }
            finally
            {
                // CRITICAL: Force clear the memory tracker so failed data doesn't bleed into the next attempt
                _context.ChangeTracker.Clear();
                await DisposeTransactionAsync();
            }
        }




        private async Task DisposeTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }






        public void Dispose()
        { 
            _context.Dispose();
        }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
           
            var typeName = typeof(TEntity).Name;
           
          return(IGenericRepository<TEntity>)  _repositories.GetOrAdd(typeName,(string ket)=> new GenericRepository<TEntity>(_context));
        }
    }
}
