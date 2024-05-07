using FoodShop.Model.Models;

namespace FoodShop.Repository.RepositoryInterface
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<int> AddOrderAsync(Order order);
        Task<List<Order>> GetOrdersByUserEmailAsync(string userEmail);
    }
}
