using FoodShop.Model.Models;
using FoodShop.Persistence;
using FoodShop.Repository.RepositoryInterface;
using Microsoft.EntityFrameworkCore;

namespace FoodShop.Repository.RepositoryImplement
{
    public class FoodRepository : Repository<Food>, IFoodRepository
    {
        private new readonly FoodShopDbContext _dbContext;

        public FoodRepository(FoodShopDbContext dbcontext) : base(dbcontext) 
        {
            _dbContext = dbcontext;
        }

        public async Task AddFoodAsync(Food food)
        {
            await _dbContext.Foods.AddAsync(food);
        }

        public void UpdateFood(Food food)
        {
            _dbContext.Foods.Update(food);
        }

        public void DeleteFood(Food food)
        {
            _dbContext.Foods.Remove(food);
        }

        public async Task<IEnumerable<Food>> GetAllFoodsAsync()
        {
            return await _dbContext.Foods.ToListAsync();
        }
    }
}
