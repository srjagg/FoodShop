using FoodShop.Model.Models;

namespace FoodShop.Core.CoreInterface
{
    public interface ILoginCore
    {
        Task<string?> IsLoggedIn(LoginModel loginModel);
        string GenerateJWTToken(User user);
    }
}
