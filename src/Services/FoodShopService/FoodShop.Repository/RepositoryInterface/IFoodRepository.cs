using FoodShop.Model.Models;

namespace FoodShop.Repository.RepositoryInterface
{
    public interface IFoodRepository : IRepository<Food>
    {
        Task<int> AddFoodAsync(Food food);
        Task<bool> UpdateFoodAsync(Food food);
        Task<bool> DeleteFoodAsync(Food food);
        Task<Food?> GetFoodByIdAsync(int id);
        Task<IEnumerable<Food>> GetAllFoodAsync();
        Task<List<Food>> GetAvailableFoods();
    }
}
