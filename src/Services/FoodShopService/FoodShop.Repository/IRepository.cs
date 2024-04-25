namespace FoodShop.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIDAsync(int id);

        Task<IEnumerable<T>> GetAllAsync();

        Task<int> InsertAsync(T entity);

        Task<bool> UpdateAsync<TEntity>(TEntity entity);

        Task<bool> Delete(T entity);

        Task<bool> Delete<TEntry>(TEntry entity);

        Task Add<TEntity>(TEntity entity);

        void ModifiedState<TEntity>(TEntity entity);

        Task<int> SaveChanges();
    }
}
