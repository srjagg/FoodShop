using FoodShop.Repository.RepositoryInterface;

namespace FoodShop.UnitOfWork
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IOrderRepository OrderRepository { get; }
        IFoodRepository FoodRepository { get; }
        IOrderDetailRepository OrderDetailRepository { get; }
        ILoginRepository LoginRepository { get; }

        void SaveChanges();
        Task SaveChangesAsync();
    }
}
