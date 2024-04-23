using FoodShop.Model.Models;
using FoodShop.Persistence;
using FoodShop.Repository.RepositoryInterface;

namespace FoodShop.Repository.RepositoryImplement
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(FoodShopDbContext context) : base(context) { }

        public async Task<int> AddOrderDetailAsync(OrderDetail orderDetail)
        {
            if (_context.OrderDetails is not null)
            {
                return await InsertAsync(orderDetail);
            }
            return 1;
        }
    }
}
