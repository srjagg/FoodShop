using FoodShop.Core.CoreInterface;
using FoodShop.Model.ModelsDto;
using FoodShop.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FoodShop.API.Inicializator
{
    public class BDInitializer : IBDInitializer
    {
        private readonly FoodShopDbContext _dbContex;
        private readonly IUserCore _userCore;

        public BDInitializer(FoodShopDbContext dbContext, IUserCore userCore)
        {
            _dbContex = dbContext;
            _userCore = userCore;
        }
        public void Initialize()
        {
            try
            {
                if (_dbContex.Database.GetPendingMigrations().Any())
                {
                    _dbContex.Database.Migrate();
                }
            }
            catch (Exception)
            {
                throw;
            }

            if (_dbContex.Users.Any()) return;

            //Crea el usuario administrador
            _userCore.AddUserAsync(new UserDto
            {
                Name = "Food Shop Admin",
                Email = "afoodshop603@gmail.com",
                Password = "admin123",
                IsAdmin = true,
            }).GetAwaiter().GetResult();

            //Crea el usuario administrador
            _userCore.AddUserAsync(new UserDto
            {
                Name = "Food Shop User",
                Email = "user@example.com",
                Password = "user123",
                IsAdmin = false,
            }).GetAwaiter().GetResult();
        }
    }
}
