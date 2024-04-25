using FoodShop.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FoodShop.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly FoodShopDbContext _context;
        public Repository(FoodShopDbContext context)
        {
            _context = context;
        }
        public async Task Add<TEntity>(TEntity entity)
        {
            await _context.AddAsync((object)entity, default(CancellationToken));
        }

        public async Task<bool> Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete<TEntry>(TEntry entity)
        {
            await Task.Yield();
            _context.Entry((object)entity).State = EntityState.Deleted;
            return true;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIDAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        public async Task<int> InsertAsync(T entity)
        {
            await _context.AddAsync(entity, default(CancellationToken));
            return await _context.SaveChangesAsync();
        }

        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }

        public void ModifiedState<TEntry>(TEntry entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task<bool> UpdateAsync<TEntity>(TEntity entity)
        {
            if (entity == null)
                return false;

            _context.Entry(entity).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
