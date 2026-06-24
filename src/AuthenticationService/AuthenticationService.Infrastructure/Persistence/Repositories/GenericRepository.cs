
using AuthenticationService.Application.Abstractions.Persistence;
using AuthenticationService.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AuthenticationService.Infrastructure.Persistence.Repositories
{
    internal class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {

        protected readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(TEntity item)
        {
           
            await _context.Set<TEntity>().AddAsync(item);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entites)
        {
            
            await _context.Set<TEntity>().AddRangeAsync(entites);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
          return  await _context.Set<TEntity>().CountAsync(predicate);
        }

        public void Delete(TEntity item)
        {
           
           _context.Set<TEntity>().Remove(item);
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
           return await _context.Set<TEntity>().AnyAsync(predicate);
        }



        public IQueryable<TEntity> GetAllAsIQueryableAsync(bool AsNoTracking = false)
        {

            if (AsNoTracking)
                return _context.Set<TEntity>().AsNoTracking();


            return _context.Set<TEntity>();
        }



        public async Task<IEnumerable<TEntity>> GetAllAsync(bool AsNoTracking = false)
        {


            if (AsNoTracking)
                return await _context.Set<TEntity>().AsNoTracking().ToListAsync();


            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity?> GetById<TKey>(TKey id) => await _context.Set<TEntity>().FindAsync(id);




        public void Update(TEntity entity)
        {

            _context.Set<TEntity>().Update(entity);
        }

        public void UpdateRange(IEnumerable<TEntity> items)
        {

            _context.Set<TEntity>().UpdateRange(items);
        }



        //-------------------------------------------------------------------
        public async Task<TResult?> FirstOrDefaultAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector)
        =>    await _context.Set<TEntity>().Where(predicate).Select(selector).FirstOrDefaultAsync();



        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = false)
        {


            if (asNoTracking)
                return await _context.Set<TEntity>().AsNoTracking().Where(predicate).FirstOrDefaultAsync();

            return await _context.Set<TEntity>().Where(predicate).FirstOrDefaultAsync();

        }





        //-----------------------------------------------------------


        public async Task<List<TResult>> ListAsync<TResult>(Expression<Func<TEntity, TResult>> selector)
        {

            return await _context.Set<TEntity>().Select(selector).ToListAsync();
        }


        public async Task<List<TResult>> ListAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector)
        {
            return await _context.Set<TEntity>().Where(predicate).Select(selector).ToListAsync();
        }



        public async Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = false)
        {



            if (asNoTracking)
                return await _context.Set<TEntity>().AsNoTracking().Where(predicate).ToListAsync();


            return await _context.Set<TEntity>().Where(predicate).ToListAsync();
        }


        //----------------------------------------------------------

        public async Task<List<TResult>> ListPaginatedAsync<TResult>(
    Expression<Func<TEntity, TResult>> selector,
    int pageNumber,
    int pageSize
 )
        {
            var query = _context.Set<TEntity>().Select(selector);

            // Apply pagination
            query = query.Skip((pageNumber - 1) * pageSize)
                         .Take(pageSize);

            return await query.ToListAsync();
        }






        public async Task<List<TResult>> ListPaginatedAsync<TResult>(Expression<Func<TEntity, bool>> predicate,
                Expression<Func<TEntity, TResult>> selector,
                int pageNumber,
                int pageSize
            )
            {
            var query =
                _context.Set<TEntity>().Where(predicate).Select(selector);

            // Apply pagination
            query = query.Skip((pageNumber - 1) * pageSize)
                         .Take(pageSize);

            return await query.ToListAsync();
        }

        //---------------------------------------------------------------------





    }
}
