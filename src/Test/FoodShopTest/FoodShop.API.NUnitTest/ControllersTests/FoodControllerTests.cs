using FoodShop.API.Controllers;
using FoodShop.Core.CoreInterface;
using FoodShop.Core.Util;
using FoodShop.Model.ModelsDto;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FoodShop.API.NUnitTest.ControllersTests
{
    [TestFixture]
    public class FoodControllerTests
    {
        private Mock<IFoodCore> _foodCoreMock;
        private FoodController _foodController;

        [SetUp]
        public void Setup()
        {
            _foodCoreMock = new Mock<IFoodCore>();
            _foodController = new FoodController(_foodCoreMock.Object);
        }

        [Test]
        public async Task AddFoodAsync_ReturnsOkResult_WhenSuccessful()
        {
            // Arrange
            var foodDto = new FoodDto
            {
                Name = "Pizza",
                Description = "Deliciosa pizza de pepperoni",
                Price = 15.99m,
                AvailableQuantity = 10
            };
            var expectedResult = new PetitionResponse<int> { Success = true, Message = "Alimento agregado exitosamente" };
            _foodCoreMock.Setup(x => x.AddFoodAsync(foodDto)).ReturnsAsync(expectedResult);

            // Act
            var result = await _foodController.AddFoodAsync(foodDto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task UpdateFoodAsync_ReturnsOkResult_WhenSuccessful()
        {
            // Arrange
            var foodId = 1;
            var foodDto = new FoodDto
            {
                Name = "Pizza Hawaiana",
                Description = "Deliciosa pizza con piña",
                Price = 17.99m,
                AvailableQuantity = 15
            };
            var expectedResult = new PetitionResponse<bool> { Success = true, Message = "Alimento actualizado exitosamente", Result = true };
            _foodCoreMock.Setup(x => x.UpdateFoodAsync(foodId, foodDto)).ReturnsAsync(expectedResult);

            // Act
            var result = await _foodController.UpdateFoodAsync(foodId, foodDto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task GetAllFoodAsync_ReturnsOkResult_WithAllFoods()
        {
            // Arrange
            var foodDtos = new List<FoodDto>
            {
                new FoodDto { FoodId = 1, Name = "Pizza", Description = "Deliciosa pizza de pepperoni", Price = 15.99m, AvailableQuantity = 10 },
                new FoodDto { FoodId = 2, Name = "Hamburguesa", Description = "Jugosa hamburguesa con queso", Price = 12.99m, AvailableQuantity = 5 }
            };
            var expectedResult = new PetitionResponse<IEnumerable<FoodDto>> { Success = true, Message = "Alimentos obtenidos exitosamente", Result = foodDtos };
            _foodCoreMock.Setup(x => x.GetAllFoodAsync()).ReturnsAsync(expectedResult);

            // Act
            var result = await _foodController.GetAllFoodAsync();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(expectedResult));
        }
    }
}
