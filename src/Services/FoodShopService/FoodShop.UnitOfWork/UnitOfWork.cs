using FoodShop.Model.Models;
using FoodShop.Persistence;
using FoodShop.Repository;

namespace FoodShop.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FoodShopDbContext _context;
        public UnitOfWork(FoodShopDbContext context) 
        {
            _context = context;
            UserRepository = new Repository<User>(_context);
            FoodRepository = new Repository<Food>(_context);
            OrderRepository = new Repository<Order>(_context);
            OrderDetailRepository = new Repository<OrderDetail>(_context);
        }

        public IRepository<User> UserRepository { get; private set; }
        public IRepository<Food> FoodRepository { get; private set; }
        public IRepository<Order> OrderRepository { get; private set; }
        public IRepository<OrderDetail> OrderDetailRepository { get; private set; }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
