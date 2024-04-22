using FoodShop.Model.Models;

namespace FoodShop.Repository.RepositoryInterface
{
    public interface IFoodRepository : IRepository<Food>
    {
        Task AddFoodAsync(Food food);
        void UpdateFood(Food food);
        void DeleteFood(Food food);
        Task<IEnumerable<Food>> GetAllFoodsAsync();
    }
}
