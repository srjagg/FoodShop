using FoodShop.Core.CoreInterface;
using FoodShop.Model.ModelsDto;
using FoodShop.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FoodShop.API.Inicializator
{
    public class BDInitializer : IBDInitializer
    {
        private readonly FoodShopDbContext _dbContext;
        private readonly IUserCore _userCore;
        private readonly ILogger<BDInitializer> _logger;

        public BDInitializer(FoodShopDbContext dbContext, IUserCore userCore, ILogger<BDInitializer> logger)
        {
            _dbContext = dbContext;
            _userCore = userCore;
            _logger = logger;
        }
        public async Task InitializeAsync()
        {
            try
            {
                if (_dbContext.Database.GetPendingMigrations().Any())
                {
                    await _dbContext.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al migrar la base de datos");
                throw;
            }

            if (!_dbContext.Users.Any())
            {
                await AddDefaultUsersAsync();
            }
        }

        private async Task AddDefaultUsersAsync()
        {
            // Agregar usuario administrador
            await _userCore.AddUserAsync(new UserDto
            {
                Name = "Food Shop Admin",
                Email = "afoodshop603@gmail.com",
                Password = "admin123",
                IsAdmin = true,
            });

            // Agregar usuario normal
            await _userCore.AddUserAsync(new UserDto
            {
                Name = "Food Shop User",
                Email = "user@example.com",
                Password = "user123",
                IsAdmin = false,
            });
        }
    }
}
