﻿using FoodShop.Model.Models;
using FoodShop.Persistence;
using FoodShop.Repository.RepositoryInterface;

namespace FoodShop.Repository.RepositoryImplement
{
    public class FoodRepository : Repository<Food>, IFoodRepository
    {
        public FoodRepository(FoodShopDbContext dbcontext) : base(dbcontext) { }

        public async Task<int> AddFoodAsync(Food food)
        {
            if(_context.Foods is not null)
            {
               return await InsertAsync(food);
            }
            return 1;
        }

        public async Task<bool> DeleteFoodAsync(Food food)
        {
            if(food is not null)
            {
                return await Delete(food);
            }
            return false;
        }

        public async Task<IEnumerable<Food>> GetAllFoodAsync()
        {
            return await GetAllAsync();
        }

        public async Task<Food?> GetByIdAsync(int id)
        {         
            var food = await GetByIDAsync(id);

            if (food is null)
                return null;

            return food;
        }

        public async Task<bool> UpdateFoodAsync(Food food)
        {
            if(food is not null)
            {
                return await UpdateAsync(food);
            }
            return false;
        }
    }
}
