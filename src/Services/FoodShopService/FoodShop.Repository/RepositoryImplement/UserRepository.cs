using FoodShop.Model.Models;
using FoodShop.Persistence;
using FoodShop.Repository.RepositoryInterface;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await GetAllAsync();
        }
        public async Task<int> AddUserAsync(User user)
        {
            if(_context.Users is not null)
            {
               await InsertAsync(user);
               return user.UserId;
            }
            return 0;
        }

        public async Task<bool> IsEmailUnique(string email, CancellationToken cancellationToken)
        {
            return !(await _context.Users.AnyAsync(u => u.Email == email, cancellationToken));
        }
    }
}
