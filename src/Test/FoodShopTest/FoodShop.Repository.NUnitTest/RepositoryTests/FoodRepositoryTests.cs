﻿using FoodShop.Model.Models;
using FoodShop.Persistence;
using FoodShop.Repository.RepositoryImplement;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FoodShop.Repository.NUnitTest.Repository
{
    [TestFixture]
    public class FoodRepositoryTests
    {
        private FoodShopDbContext _context;
        private FoodRepository _foodRepository;

        private Food food1;
        private Food food2;

        private Food CreateFood(int foodId, string name, string description, decimal price, int availableQuantity)
        {
            return new Food
            {
                FoodId = foodId,
                Name = name,
                Description = description,
                Price = price,
                AvailableQuantity = availableQuantity
            };
        }

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<FoodShopDbContext>()
                .UseInMemoryDatabase(databaseName: "FoodShopTestDatabase")
                .Options;

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddInMemoryCollection()
                .Build();

            _context = new FoodShopDbContext(configuration, options);

            _foodRepository = new FoodRepository(_context);

            food1 = CreateFood(1, "Solomo", "Libra de Carne de alta calidad", 15000, 100);
            food2 = CreateFood(2, "Yuca", "Libra de yuca campesina", 3000, 100);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        [Order(1)]
        public async Task AddFoodAsync_ShouldAddNewFood()
        {
            // Arrange
            var food = new Food
            {
                Name = "Pizza",
                Description = "Pizza paisa",
                Price = 15000,
                AvailableQuantity = 10
            };

            // Act
            var result = await _foodRepository.AddFoodAsync(food);

            // Assert
            Assert.That(result, Is.EqualTo(1));
            Assert.That(_context.Foods.Count(), Is.EqualTo(1));
        }

        [Test]
        [Order(2)]
        public async Task FoodRepository_AddFoodAsync_SuccessfulSave()
        {
            //Arrange
            _context.Database.EnsureDeleted();

            //Act
            await _foodRepository.AddFoodAsync(food1);
            var foodBd = await _foodRepository.GetFoodByIdAsync(food1.FoodId);

            //Assert
            Assert.That(foodBd?.FoodId, Is.EqualTo(food1.FoodId));
            Assert.That(foodBd.Name, Is.EqualTo(food1.Name));
        }

        [Test]
        [Order(3)]
        public async Task FoodRepository_GetAllFood()
        {
            //Arrange
            var expectedResult = new List<Food> { food1, food2 };
            _context.Database.EnsureDeleted();

            //Act
            await _foodRepository.AddFoodAsync(food1);
            await _foodRepository.AddFoodAsync(food2);
            var foodList = await _foodRepository.GetAllFoodAsync();

            //Assert
            Assert.That(foodList, Is.EqualTo(expectedResult).AsCollection);
        }

        [Test]
        [Order(4)]
        public async Task DeleteFoodAsync_ShouldDeleteFood()
        {
            // Arrange
            _context.Database.EnsureDeleted();
            await _foodRepository.AddFoodAsync(food1);

            // Act
            var result = await _foodRepository.DeleteFoodAsync(food1);

            // Assert
            Assert.IsTrue(result);
            Assert.That(_context.Foods.Count(), Is.EqualTo(0));
        }

        [Test]
        [Order(5)]
        public async Task UpdateFoodAsync_ShouldUpdateFood()
        {
            // Arrange
            _context.Database.EnsureDeleted();
            await _foodRepository.AddFoodAsync(food1);

            food1.FoodId = food1.FoodId;
            food1.Name = "Pizza";
            food1.Description = "Pizza paisa extra grande";
            food1.Price = 20000;
            food1.AvailableQuantity = 1;

            // Act
            var result = await _foodRepository.UpdateFoodAsync(food1);

            // Assert
            Assert.IsTrue(result);

            var foodFromDb = await _foodRepository.GetFoodByIdAsync(food1.FoodId);
            Assert.IsNotNull(foodFromDb);
            Assert.That(foodFromDb.Name, Is.EqualTo(food1.Name));
            Assert.That(foodFromDb.AvailableQuantity, Is.EqualTo(food1.AvailableQuantity));
        }
    }
}
