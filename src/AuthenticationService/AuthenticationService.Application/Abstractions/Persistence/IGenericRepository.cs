

using System.Linq.Expressions;

namespace AuthenticationService.Application.Abstractions.Persistence
{
    public interface IGenericRepository<TEntity> 
    where TEntity : class
    {


        //Write 

        Task AddAsync(TEntity item);


        Task AddRangeAsync(IEnumerable<TEntity> entites);
        void Update(TEntity entity);

        void UpdateRange(IEnumerable<TEntity> items);

        void Delete(TEntity item);

        Task<TEntity?> GetById<TKey>(TKey id);
        Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking = false);

        IQueryable<TEntity> GetAllAsIQueryableAsync(bool asNoTracking = false);
        //----------------------------------------------

        //Read



        Task<TResult?> FirstOrDefaultAsync<TResult>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> selector
           );


        Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate, bool asNoTracking = false

            );


 



        Task<List<TResult>> ListAsync<TResult>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> selector
            );


        Task<List<TEntity>> ListAsync(
    Expression<Func<TEntity, bool>> predicate, bool asNoTracking = false
    );


        Task<List<TResult>> ListAsync<TResult>(
            Expression<Func<TEntity, TResult>> selector
           );



        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);



        public Task<List<TResult>> ListPaginatedAsync<TResult>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> selector,
            int pageNumber,
            int pageSize
            );



        public  Task<List<TResult>> ListPaginatedAsync<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            int pageNumber,
            int pageSize
            );



    }


}
