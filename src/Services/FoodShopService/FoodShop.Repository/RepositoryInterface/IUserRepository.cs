﻿using FoodShop.Model.Models;

namespace FoodShop.Repository.RepositoryInterface
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetUserByIdAsync(int id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<int> AddUserAsync(User user);
        Task<bool> IsEmailUnique(string email, CancellationToken cancellationToken);
    }
}
