using FoodShop.Model.Models;
using FoodShop.Persistence;
using FoodShop.Repository.RepositoryInterface;

namespace FoodShop.Repository.RepositoryImplement
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(FoodShopDbContext context) : base(context){ }

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
