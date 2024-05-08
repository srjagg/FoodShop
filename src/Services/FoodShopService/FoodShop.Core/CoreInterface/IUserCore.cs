using FoodShop.Core.Util;
using FoodShop.Model.ModelsDto;

namespace FoodShop.Core.CoreInterface
{
    public interface IUserCore
    {
        Task<PetitionResponse<IEnumerable<UserDto>>> GetAllUsersAsync();
        Task<PetitionResponse<int>> AddUserAsync(UserDto userModel);
    }
}
