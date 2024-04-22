using FoodShop.Model.Models;
using FoodShop.Persistence;
using FoodShop.Repository.RepositoryInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Repository.RepositoryImplement
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly FoodShopDbContext _context;
        public OrderRepository(FoodShopDbContext dbContext) : base(dbContext)
        { 
            _context = dbContext;
        }
    }
}
