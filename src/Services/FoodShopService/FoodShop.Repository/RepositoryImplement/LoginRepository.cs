using FoodShop.Model.Models;
using FoodShop.Persistence;
using FoodShop.Repository.RepositoryInterface;
using Microsoft.EntityFrameworkCore;

namespace FoodShop.Repository.RepositoryImplement
{
    public class LoginRepository : Repository<User>, ILoginRepository
    {
        public LoginRepository(FoodShopDbContext dbcontext) : base(dbcontext) { }

        public async Task<User?> IsLoggedIn(string email)
        {
            var entity = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);

            if (entity is null)
                return null;

            return entity;
        }
    }
}
