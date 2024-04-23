using FoodShop.Model.Models;
using FoodShop.Persistence;
using FoodShop.Repository.RepositoryInterface;

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
    }
}
