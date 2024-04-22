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
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private readonly FoodShopDbContext _context;

        public OrderDetailRepository(FoodShopDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
