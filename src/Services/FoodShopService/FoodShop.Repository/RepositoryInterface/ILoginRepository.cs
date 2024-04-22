using FoodShop.Model.Models;

namespace FoodShop.Repository.RepositoryInterface
{
    public interface ILoginRepository
    {
        Task<User?> IsLoggedIn(string email);
    }
}
