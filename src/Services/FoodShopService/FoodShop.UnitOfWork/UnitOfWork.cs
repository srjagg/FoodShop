﻿using FoodShop.Persistence;
using FoodShop.Repository.RepositoryImplement;
using FoodShop.Repository.RepositoryInterface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FoodShop.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FoodShopDbContext _context;
        private bool _disposed = false;

        public UnitOfWork(IConfiguration config, DbContextOptions<FoodShopDbContext> options)
        {
            _context = new FoodShopDbContext(config, options);
            UserRepository = new UserRepository(_context);
            OrderRepository = new OrderRepository(_context);
            FoodRepository = new FoodRepository(_context);
            LoginRepository = new LoginRepository(_context);
        }

        public IUserRepository UserRepository { get; }
        public IOrderRepository OrderRepository { get; }
        public IFoodRepository FoodRepository { get; }
        public ILoginRepository LoginRepository { get; }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
