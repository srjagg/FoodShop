namespace FoodShop.Repository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        T GetById(int id);
        Task<T> GetByIdAsync(int id);
        void Insert(T entity);
        Task InsertAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
