using FoodShop.Core.Util;
using FoodShop.Model.ModelsDto;

namespace FoodShop.Core.CoreInterface
{
    public interface IFoodCore
    {
        Task<PetitionResponse<int>> AddFoodAsync(FoodDto foodDto);
        Task<PetitionResponse<bool>> UpdateFoodAsync(int foodId, FoodDto foodDto);
        Task<PetitionResponse<bool>> DeleteFoodAsync(int foodId);
        Task<PetitionResponse<IEnumerable<FoodDto>>> GetAllFoodAsync();
        Task<PetitionResponse<List<FoodDto>>> GetAvailableFoods();
    }
}
