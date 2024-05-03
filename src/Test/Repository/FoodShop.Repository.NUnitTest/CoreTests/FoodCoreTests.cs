using FluentValidation.TestHelper;
using FoodShop.Core.CoreImplement;
using FoodShop.Core.FluentValidation;
using FoodShop.Model.Models;
using FoodShop.Model.ModelsDto;
using FoodShop.UnitOfWork;
using Moq;
using NUnit.Framework;

namespace FoodShop.NUnitTest.CoreTests
{
    [TestFixture]
    public class FoodCoreTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private FoodValidator _foodValidator;
        private FoodCore _foodCore;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _foodValidator = new FoodValidator();
            _foodCore = new FoodCore(_unitOfWorkMock.Object, _foodValidator);
        }

        [Test]
        public async Task AddFoodAsync_ReturnsSuccessResponse_WhenValidationSucceeds()
        {
            // Arrange
            var foodDto = new FoodDto
            {
                Name = "Pizza",
                Description = "Deliciosa pizza de pepperoni",
                Price = 15.99m,
                AvailableQuantity = 10
            };
            _unitOfWorkMock.Setup(x => x.FoodRepository.AddFoodAsync(It.IsAny<Food>())).ReturnsAsync(1);

            // Act
            var result = await _foodCore.AddFoodAsync(foodDto);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual("Alimento agregado exitosamente", result.Message);
        }

        [Test]
        public async Task GetAllFoodAsync_ReturnsAllFoods()
        {
            // Arrange
            var foods = new List<Food>
            {
                new Food { FoodId = 1, Name = "Pizza", Description = "Deliciosa pizza de pepperoni", Price = 15.99m, AvailableQuantity = 10 },
                new Food { FoodId = 2, Name = "Hamburguesa", Description = "Jugosa hamburguesa con queso", Price = 12.99m, AvailableQuantity = 5 }
            };
            _unitOfWorkMock.Setup(x => x.FoodRepository.GetAllAsync()).ReturnsAsync(foods);

            // Act
            var result = await _foodCore.GetAllFoodAsync();

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual(2, result.Result.Count());
        }
    }
}
