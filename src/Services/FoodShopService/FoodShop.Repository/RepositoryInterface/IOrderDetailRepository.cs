using FoodShop.Model.Models;

namespace FoodShop.Repository.RepositoryInterface
{
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
        Task<int> AddOrderDetailAsync(OrderDetail orderDetail);
    }
}
