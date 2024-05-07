using FoodShop.Model.Models;
using FoodShop.Persistence;
using FoodShop.Repository.RepositoryInterface;
using Microsoft.EntityFrameworkCore;

namespace FoodShop.Repository.RepositoryImplement
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(FoodShopDbContext dbContext) : base(dbContext) { }

        public async Task<int> AddOrderAsync(Order order)
        {
            if (_context.Orders is not null)
            {
                var orderId = await InsertAsync(order);
                return orderId;
            }
            return 0;
        }

        public async Task<List<Order>> GetOrdersByUserEmailAsync(string userEmail)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Food)
                .Where(o => o.User.Email == userEmail)
                .ToListAsync();
        }
    }
}
