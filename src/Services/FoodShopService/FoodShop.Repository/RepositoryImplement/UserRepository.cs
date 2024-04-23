using FoodShop.Model.Models;
using FoodShop.Persistence;
using FoodShop.Repository.RepositoryInterface;

namespace FoodShop.Repository.RepositoryImplement
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(FoodShopDbContext context) : base(context){ }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            var user = await GetByIDAsync(id);

            if (user is null)
                return null;

            return user;
        }
        public async Task<int> AddUserAsync(User user)
        {
            if(_context.Users is not null)
            {
               return await InsertAsync(user);
            }
            return 1;
        }
    }
}
