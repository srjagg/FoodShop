using FoodShop.Core.Util;
using FoodShop.Model.ModelsDto;

namespace FoodShop.Core.CoreInterface
{
    public interface IUserCore
    {
        Task<PetitionResponse<int>> AddUserAsync(UserDto userModel);
    }
}
