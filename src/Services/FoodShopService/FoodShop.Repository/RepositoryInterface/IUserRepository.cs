using FoodShop.Model.Models;

namespace FoodShop.Repository.RepositoryInterface
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetUserByIdAsync(int id);
        Task<int> AddUserAsync(User user);
    }
}
