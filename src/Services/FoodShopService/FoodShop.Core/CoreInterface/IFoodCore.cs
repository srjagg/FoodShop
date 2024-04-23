using FoodShop.Core.Util;
using FoodShop.Model.Models;

namespace FoodShop.Core.CoreInterface
{
    public interface IFoodCore
    {
        Task<PetitionResponse<int>> AddFoodAsync(Food food);
        Task<PetitionResponse<bool>> UpdateFoodAsync(int foodId, Food food);
        Task<PetitionResponse<bool>> DeleteFoodAsync(int foodId);
        Task<PetitionResponse<IEnumerable<Food>>> GetAllFoodAsync();
    }
}
