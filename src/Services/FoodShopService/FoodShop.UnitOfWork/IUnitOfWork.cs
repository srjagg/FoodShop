using FoodShop.Model.Models;
using FoodShop.Repository;

namespace FoodShop.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<User> UserRepository { get; }
        IRepository<Food> FoodRepository { get; }
        IRepository<Order> OrderRepository { get; }
        IRepository<OrderDetail> OrderDetailRepository { get; }

        void SaveChanges();
        Task SaveChangesAsync();
    }
}
