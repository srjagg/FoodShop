using FoodShop.Model.Models;

namespace FoodShop.Repository.RepositoryInterface
{
    public interface IUserRepository : IRepository<User>
    {
        Task<int> AddUserAsync(User user);
    }
}
